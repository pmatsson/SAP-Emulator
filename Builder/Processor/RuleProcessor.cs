using Builder.Model.Action;
using Builder.Model.Condition;
using Builder.Model.Trigger;
using Builder.MQ;
using Builder.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            RuleProcessed += RuleProcessor_RuleProcessed;
        }

        private void RuleProcessor_RuleProcessed(object sender, Rule e)
        {
            foreach (var trigger in e.TriggerGroup.Triggers)
            {
                if(trigger.Selected is OnceTrigger)
                {
                    (trigger.Selected as OnceTrigger).HitCount++;
                }
            }
        }

        public async Task<bool> Start()
        {
            foreach(Rule rule in _rules)
            {
                foreach (Trigger trigger in rule.TriggerGroup.Triggers)
                {
                    if (trigger.Selected is OnceTrigger)
                    {
                        (trigger.Selected as OnceTrigger).HitCount = 0;
                    }
                }

                ProcessRules();
            }

            await Task.Delay(1000);

            while (!_wtoken.IsCancellationRequested)
            {
                ProcessMessage();
                await Task.Delay(1000);
            }

            return true;
        }



        private void ProcessMessage()
        {
            string msg = "";
            if (_mqhandler.Read(ref msg))
            {
                XmlDocument doc = CreateXmlDoc(msg);
                if (doc != null)
                {
                    ProcessRules(doc);
                }
            }
        }

        public void ProcessRules(XmlDocument doc = null)
        {
            Rule ruleOut = null;
            if (CheckTrigger(doc, ref ruleOut) && CheckCondition(doc, ruleOut.ConditionGroup))
            {
                DoAction(doc, ruleOut.ActionGroup);
                RuleProcessed(this, ruleOut);
            }
        }

        public void Cancel()
        {
            _wtoken.Cancel();
        }


        public XmlDocument CreateXmlDoc(string msg)
        {
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(msg);
            }
            catch (XmlException ex)
            {
                return null;
            }

            return doc;
        }

        public bool CheckTrigger(XmlDocument doc, ref Rule ruleOut)
        {
            foreach (Rule rule in _rules)
            {
                var hitList = new Collection<bool>();
                foreach (var trigger in rule.TriggerGroup.Triggers)
                {
                    if (trigger.Selected is OnceTrigger)
                    {
                        hitList.Add((trigger.Selected as OnceTrigger).HitCount == 0);
                    }
                    else if(trigger.Selected is ReceivedTrigger)
                    {
                        hitList.Add(((trigger.Selected as ReceivedTrigger).GetDocumentType() == doc?.DocumentElement.Name));
                    }
                }

                if (hitList.Count > 0 && !hitList.Contains(false))
                {
                    ruleOut = rule;
                    return true;
                }
            }

            return false;
        }

        public bool CheckCondition(XmlDocument doc, ConditionGroup cg)
        {
            var hitList = new Collection<bool>();
            foreach (var condition in cg.Conditions)
            {
                if (condition.Selected is AlwaysCondition)
                {
                    hitList.Add(true);
                }
                else if (condition.Selected is ContainsCondition)
                {
                    bool containHit = false;
                    var nodes = doc.SelectNodes(condition.Selected.Param1);
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        var node = nodes.Item(i);
                        if (node.ChildNodes.Count == 1 && (node.FirstChild is XmlText))
                        {
                            if (nodes.Item(i).InnerText == condition.Selected.Param2)
                            {
                                containHit = true;
                                break;
                            }
                        }
                    }
                    hitList.Add(containHit);
                }
            }

            return (hitList.Count > 0 && !hitList.Contains(false));
        }


        public void DoAction(XmlDocument doc, ActionGroup ag)
        {

            foreach(var action in ag.Actions)
            {
                if(action.Selected is SendAction)
                {
                    string text = System.IO.File.ReadAllText((action.Selected as SendAction).GetFilePath());
                    _mqhandler.Write(text);
                }
            }
        }

        public void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(GetTempPath() + "debug.txt");
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
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }
    }
}
