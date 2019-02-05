using MQChatter.Common;
using MQChatter.Model.Action;
using MQChatter.Model.Condition;
using MQChatter.Model.Trigger;
using MQChatter.Processor;
using MQChatter.ViewModel.RuleGroup.Action;
using MQChatter.ViewModel.RuleGroup.Condition;
using MQChatter.ViewModel.RuleGroup.Trigger;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Action = MQChatter.ViewModel.RuleGroup.Action.Action;

namespace MQChatter.ViewModel
{
    public class Rule : NotifyPropertyChangedBase
    {
        public TriggerGroup TriggerGroup { get; set; }

        public ConditionGroup ConditionGroup { get; set; }

        public ActionGroup ActionGroup { get; set; }

        public int ProcessCount { get; set; }

        public Rule()
        {
            TriggerGroup = new TriggerGroup();
            ConditionGroup = new ConditionGroup();
            ActionGroup = new ActionGroup();

            TriggerGroup.AddTrigger();
            ConditionGroup.AddCondition();
            ActionGroup.AddAction();
        }

        public void Reset()
        {
            ProcessCount = 0;
            foreach (Trigger trigger in TriggerGroup.Triggers)
            {
                trigger.Selected.Reset();
            }

            foreach (Condition condition in ConditionGroup.Conditions)
            {
                condition.Selected.Reset();
            }

            foreach (Action action in ActionGroup.Actions)
            {
                action.Selected.Reset();
            }
        }
    }

    public class RuleViewModel : NotifyPropertyChangedBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<Rule> _rules;
        private int _processedRulesCount;
        private RuleProcessor _ruleProcessor;
        private bool _isRunningEmulator;
        private string _openDocument;
        private Rule _selectedRule;

        public ObservableCollection<Rule> Rules
        {
            get => _rules;
            set => SetProperty(ref _rules, value);
        }

        public Rule SelectedRule
        {
            get => _selectedRule;
            set => SetProperty(ref _selectedRule, value);
        }

        public int ProcessedRuleCount
        {
            get => _processedRulesCount;
            set => SetProperty(ref _processedRulesCount, value);
        }

        public bool IsRunningEmulator
        {
            get => _isRunningEmulator;
            set => SetProperty(ref _isRunningEmulator, value);
        }

        public string OpenDocument
        {
            get => _openDocument;
            set => SetProperty(ref _openDocument, value);
        }

        public RuleViewModel()
        {
            Rules = new ObservableCollection<Rule>();
            _isRunningEmulator = false;
        }

        public async void StartEmulation()
        {
            IsRunningEmulator = true;
            _ruleProcessor = new RuleProcessor(Rules.ToList());
            _ruleProcessor.RuleProcessed += RuleProcessor_RuleProcessed;
            _ruleProcessor.MessageReceived += _ruleProcessor_MessageReceived;

            // Logging configuration
            NLog.Config.LoggingConfiguration config = new NLog.Config.LoggingConfiguration();
            NLog.Targets.FileTarget logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "emulation_" + DateTime.Now.DayOfYear.ToString() + ".log"
            };

            NLog.Targets.ConsoleTarget logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Warn, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;

            try
            {
                await _ruleProcessor.Start();
            }
            catch
            {
                _ruleProcessor.Stop();
                IsRunningEmulator = false;
            }
        }

        private void _ruleProcessor_MessageReceived(object sender, string s)
        {
            logger.Trace("Retrieved message: {0}", s);
        }

        public void CancelEmulation()
        {
            if (_ruleProcessor != null)
            {
                _ruleProcessor.Stop();
                ProcessedRuleCount = 0;
                IsRunningEmulator = false;
            }
        }

        private void RuleProcessor_RuleProcessed(object sender, Rule rule)
        {
            rule.ProcessCount++;
            ProcessedRuleCount = Rules.Where(x => x?.ProcessCount > 0).Count();
            logger.Trace("Rule " + rule.GetHashCode() + " processed. This rule has been processed {0} time(s)", rule.ProcessCount.ToString());
        }

        public void CreateRule()
        {
            Rules.Add(new Rule());
        }

        public void RemoveSelectedRule()
        {
            if (SelectedRule != null)
            {
                Rules.Remove(SelectedRule);
            }
        }

        public void SerializeRules(string filename)
        {
            TextWriter writer = new StreamWriter(filename);
            RuleSerializer.Serialize(Rules.ToList(), writer);
        }

        public void DeSerializeRules(string filename)
        {
            Rules = RuleSerializer.Deserialize(filename);
        }

        public static class RuleSerializer
        {
            public static void Serialize(List<Rule> rules, TextWriter stream)
            {
                List<Type> ruleTypes = new List<Type>();
                Rule rule = new Rule();

                foreach (TriggerBase trigger in rule.TriggerGroup.Triggers.First().AvailableTriggers)
                {
                    Type type = trigger.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (ConditionBase condition in rule.ConditionGroup.Conditions.First().AvailableConditions)
                {
                    Type type = condition.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (ActionBase action in rule.ActionGroup.Actions.First().AvailableActions)
                {
                    Type type = action.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<Rule>), ruleTypes.ToArray());
                serializer.Serialize(stream, rules);
                stream.Close();
            }

            public static ObservableCollection<Rule> Deserialize(string path)
            {
                List<Type> ruleTypes = new List<Type>();
                Rule rule = new Rule();

                foreach (TriggerBase trigger in rule.TriggerGroup.Triggers.First().AvailableTriggers)
                {
                    Type type = trigger.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (ConditionBase condition in rule.ConditionGroup.Conditions.First().AvailableConditions)
                {
                    Type type = condition.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (ActionBase action in rule.ActionGroup.Actions.First().AvailableActions)
                {
                    Type type = action.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                ObservableCollection<Rule> result = null;
                TextReader reader = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Rule>), ruleTypes.ToArray());
                try
                {
                    result = (ObservableCollection<Rule>)serializer.Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                    // TODO: Handle exception
                    return null;
                }
                finally
                {
                    reader.Close();
                }

                return result;
            }
        }
    }
}