using MQChatter.Model.Trigger;
using MQChatter.MQ;
using MQChatter.ViewModel;
using MQChatter.ViewModel.RuleGroup.Action;
using MQChatter.ViewModel.RuleGroup.Condition;
using MQChatter.ViewModel.RuleGroup.Trigger;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Action = MQChatter.ViewModel.RuleGroup.Action.Action;

namespace MQChatter.Processor
{
    public class RuleProcessor
    {
        private List<Rule> _rules;
        private Dictionary<MQProps, MQHandler> _recvHandlers;
        private Dictionary<MQProps, XmlDocument> _recvFront;
        private CancellationTokenSource _wtoken;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public event EventHandler<Rule> RuleProcessed;

        public event EventHandler<string> MessageReceived;

        public RuleProcessor(List<Rule> rules)
        {
            _rules = rules;
            _recvHandlers = new Dictionary<MQProps, MQHandler>();
            _recvFront = new Dictionary<MQProps, XmlDocument>();
            _wtoken = new CancellationTokenSource();
        }

        public async Task Start()
        {
            logger.Debug("Starting emulation");

            // Restore default values
            _rules.ForEach(x => x.Reset());

            // Create MQ handlers
            foreach (Rule rule in _rules)
            {
                foreach (Trigger trigger in rule.TriggerGroup.Triggers)
                {
                    if (trigger.Selected is ReceivedTrigger)
                    {
                        CreateMQHandler((trigger.Selected as ReceivedTrigger).MQSettings);
                    }
                }
            }

            // Start processing rules
            while (!_wtoken.IsCancellationRequested)
            {
                try
                {
                    await ProcessRules();
                }
                catch (Exception ex)
                {
                    logger.Warn("Unhandled exception was caught. Throwing: {0}", ex.Message);
                    throw;
                }
            }
        }

        private void CreateMQHandler(MQProps props)
        {
            if (!_recvHandlers.ContainsKey(props))
            {
                logger.Debug("MQ handler created. qm: {0} q: {1} ch: {2} h: {3} p: {4}",
                    props.QueueManagerName, props.QueueName, props.ChannelName, props.Hostname, props.Port);
                _recvHandlers.Add(props, new MQHandler());
            }
        }

        private void PopQueues(ref Dictionary<MQProps, XmlDocument> docs, Dictionary<MQProps, MQHandler> handlers)
        {
            docs.Clear();

            foreach (KeyValuePair<MQProps, MQHandler> handler in handlers)
            {
                // Establish connection to MQ
                if (!handler.Value.IsConnected())
                {
                    if (!handler.Value.Connect(handler.Key))
                    {
                        logger.Fatal("Was not able to connect. qm: {0} q: {1} ch: {2} h: {3} p: {4}",
                            handler.Key.QueueManagerName, handler.Key.QueueName, handler.Key.ChannelName, handler.Key.Hostname, handler.Key.Port);
                        throw new ArgumentException("Was not able to connect to the specified MQ instance");
                    }
                }

                string msg = "";
                XmlDocument doc = new XmlDocument();

                // Pop message queue
                if (handler.Value.Read(ref msg))
                {
                    MessageReceived(this, msg);

                    try
                    {
                        doc.LoadXml(msg);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex, "Received message contains invalid xml markup");
                    }
                }

                // Store message with the associated handler
                docs[handler.Key] = doc;
            }
        }

        public async Task ProcessRules()
        {
            try
            {
                // Read messages from queues
                PopQueues(ref _recvFront, _recvHandlers);
            }
            catch
            {
                throw;
            }

            foreach (Rule rule in _rules)
            {
                // Check trigger, retrieve xml if applicable
                if (CheckTriggers(out XmlDocument doc, rule.TriggerGroup, rule.ProcessCount))
                {
                    logger.Debug("Fulfilled trigger for rule {0}", rule.GetHashCode());

                    // Check conditions
                    if (CheckConditions(doc, rule.ConditionGroup, rule.ProcessCount))
                    {
                        logger.Debug("Fulfilled condition for rule {0}", rule.GetHashCode());
                        // Perform action
                        DoActions(doc, rule.ActionGroup, rule.ProcessCount);
                        RuleProcessed(this, rule);
                    }
                }
            }

            await Task.Delay(1000);
        }

        public void Stop()
        {
            logger.Debug("Stopping emulation...");
            _wtoken.Cancel();
        }

        public bool CheckTriggers(out XmlDocument doc, TriggerGroup tg, int processCount)
        {
            doc = null;
            Collection<bool> trigResults = new Collection<bool>();
            foreach (Trigger trigger in tg.Triggers)
            {
                if (trigger.Selected is ReceivedTrigger)
                {
                    ReceivedTrigger rt = (trigger.Selected as ReceivedTrigger);

                    // Retrieve the current document from the queue
                    doc = _recvFront[rt.MQSettings];
                    trigResults.Add(trigger.Selected.TryProcess(doc, processCount, tg));
                }
                else
                {
                    trigResults.Add(trigger.Selected.TryProcess(null, processCount, tg));
                }
            }

            // We have > 0 triggers and all triggers in TG are fulfilled
            return (trigResults.Count > 0 && !trigResults.Contains(false));
        }

        public bool CheckConditions(XmlDocument doc, ConditionGroup cg, int processCount)
        {
            Collection<bool> condResults = new Collection<bool>();
            foreach (Condition condition in cg.Conditions)
            {
                condResults.Add(condition.Selected.TryProcess(doc, processCount, cg));
            }

            // We have > 0 conditions and all conditions in CG are fulfilled
            return (condResults.Count > 0 && !condResults.Contains(false));
        }

        public void DoActions(XmlDocument doc, ActionGroup ag, int processCount)
        {
            foreach (Action action in ag.Actions)
            {
                logger.Debug("Performing action: {0}", action.Selected.DisplayName);

                action.Selected.TryProcess(doc, processCount, ag);
            }
        }
    }
}