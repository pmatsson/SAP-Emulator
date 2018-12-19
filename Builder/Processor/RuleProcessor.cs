using Builder.Model.Action;
using Builder.Model.Condition;
using Builder.Model.Trigger;
using Builder.MQ;
using Builder.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Builder.Processor
{
    public class RuleProcessor
    {
        List<Rule> _rules;
        MQHandler _mqhandler;

        public RuleProcessor(List<Rule> rules, MQHandler mqhandler)
        {
            _rules = rules;
            _mqhandler = mqhandler;

            string text = System.IO.File.ReadAllText(@"C:\Users\cmatsson\Projects\MSS\OCI\IMS5\OCI_instruction.xml");
            _mqhandler.Write(text);
            Start();
        }


        public void Start()
        {
            while(true)
            {
                string msg = "";
                if(_mqhandler.Read(ref msg))
                {
                    Rule rule = null;
                    XmlDocument doc = Parse(msg);

                    if(CheckTrigger(doc, ref rule) && CheckCondition(doc, rule.ConditionGroup))
                    {
                        DoAction(doc, rule.ActionGroup);
                    }
                }

                Thread.Sleep(500);
            }
        }


        public XmlDocument Parse(string msg)
        {
            var doc = new XmlDocument();
            doc.LoadXml(msg);
            return doc;
        }

        public bool CheckTrigger(XmlDocument doc, ref Rule rhit)
        {
            foreach (Rule rule in _rules)
            {
                var hitList = new Collection<bool>();
                foreach (var trigger in rule.TriggerGroup.Triggers)
                {
                    if (trigger.Selected is OnceTrigger)
                    {
                        hitList.Add((trigger.Selected as OnceTrigger).HitCount++ == 0);
                    }
                    else if(trigger.Selected is ReceivedTrigger)
                    {
                        hitList.Add(((trigger.Selected as ReceivedTrigger).GetDocumentType() == doc.DocumentElement.Name));
                    }
                }

                if (hitList.Count > 0 && !hitList.Contains(false))
                {
                    rhit = rule;
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

    }
}
