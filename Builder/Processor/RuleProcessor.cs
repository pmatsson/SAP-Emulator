using Builder.Model.Action;
using Builder.Model.Condition;
using Builder.Model.Trigger;
using Builder.MQ;
using Builder.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Builder.Processor
{
    public class RuleProcessor
    {
        private List<Rule> _rules;
        private MQHandler _mqhandler;
        private CancellationTokenSource _wtoken;

        public event EventHandler<Rule> RuleProcessed;

        public RuleProcessor(List<Rule> rules, MQHandler mqhandler)
        {
            _rules = rules;
            _mqhandler = mqhandler;
            _wtoken = new CancellationTokenSource();
        }

        public async Task<bool> Start()
        {
            // Restore rules to default values
            foreach(Rule rule in _rules)
            {
                rule.Reset();
            }

            while (!_wtoken.IsCancellationRequested)
            {
                ProcessRules();
                await Task.Delay(1000);
            }

            return true;
        }



        //private XmlDocument ReadMessage()
        //{
        //    XmlDocument result = null;
        //    string msg = "";
        //    if (_mqhandler.Read(ref msg))
        //    {
        //        result = CreateXmlDoc(msg);
        //    }
        //    return result;
        //}

        public void ProcessRules()
        {
            foreach(Rule rule in _rules)
            {
                XmlDocument doc;
                if (CheckTriggers(out doc, rule.TriggerGroup, rule.ProcessCount))
                {
                    if (CheckConditions(doc, rule.ConditionGroup, rule.ProcessCount))
                    {
                        DoActions(doc, rule.ActionGroup, rule.ProcessCount);
                        RuleProcessed(this, rule);
                    }
                }
            }
        }

        public void Cancel()
        {
            _wtoken.Cancel();
        }


        //public XmlDocument CreateXmlDoc(string msg)
        //{
        //    var doc = new XmlDocument();
        //    try
        //    {
        //        doc.LoadXml(msg);
        //    }
        //    catch (XmlException ex)
        //    {
        //        return null;
        //    }

        //    return doc;
        //}

        public bool CheckTriggers(out XmlDocument doc, TriggerGroup tg, int processCount)
        {
            doc = null;
            var trigResults = new Collection<bool>();
            foreach (var trigger in tg.Triggers)
            {
                if(trigger.Selected is ReceivedTrigger)
                {
                    var rt = (trigger.Selected as ReceivedTrigger);
                    trigResults.Add(rt.Process(out doc, processCount));
                }
                else
                {
                    trigResults.Add(trigger.Selected.TryProcess(doc, processCount));
                }
            }

            return (trigResults.Count > 0 && !trigResults.Contains(false));
        }

        public bool CheckConditions(XmlDocument doc, ConditionGroup cg, int processCount)
        {
            var condResults = new Collection<bool>();
            foreach (var condition in cg.Conditions)
            {
                condResults.Add(condition.Selected.TryProcess(doc, processCount));

            }
            return (condResults.Count > 0 && !condResults.Contains(false));
        }


        public void DoActions(XmlDocument doc, ActionGroup ag, int processCount)
        {
            foreach (var action in ag.Actions)
            {
                action.Selected.TryProcess(doc, processCount);
            }
        }


        public void LogMessageToFile(string msg)
        {
            StreamWriter sw = File.AppendText(GetTempPath() + "debug.txt");
            try
            {
                string logLine = System.String.Format("{0:G}: {1}.", DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

        public string GetTempPath()
        {
            string path = Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }
    }
}
