using Builder.Model.Action;
using Builder.Model.Condition;
using Builder.Model.Trigger;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Builder.ViewModel
{

    public class Rule : ViewModelBase
    {
        [XmlElement("Triggers")]
        public TriggerGroup TriggerGroup { get; set; }

        [XmlElement("Conditions")]
        public ConditionGroup ConditionGroup { get; set; }

        [XmlElement("Actions")]
        public ActionGroup ActionGroup { get; set; }

        public Rule()
        {
            TriggerGroup = new TriggerGroup();
            ConditionGroup = new ConditionGroup();
            ActionGroup = new ActionGroup();

            TriggerGroup.AddTrigger();
            ConditionGroup.AddCondition();
            ActionGroup.AddAction();
        }


    }

    public class RuleViewModel : ViewModelBase
    {
        private ObservableCollection<Rule> _rules;

        public ObservableCollection<Rule> Rules
        {
            get => _rules;
            set => SetProperty(ref _rules, value);
        }

        public RuleViewModel()
        {
            Rules = new ObservableCollection<Rule>();

        }

        public void AddRow()
        {
            Rules.Add(new Rule());
        }

        public void SerializeRules()
        {
            TextWriter writer = new StreamWriter("test.xml");
            RuleSerializer.Serialize(Rules.ToList(), writer);

        }

        public void DeSerializeRules()
        {
            Rules = RuleSerializer.Deserialize("test.xml");
        }

        public static class RuleSerializer
        {
            public static void Serialize(List<Rule> rules, TextWriter stream)
            {
                List<Type> ruleTypes = new List<Type>();
                var rule = new Rule();


                foreach (var trigger in rule.TriggerGroup.Triggers.First().AvailableTriggers)
                {
                    var type = trigger.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (var condition in rule.ConditionGroup.Conditions.First().AvailableConditions)
                {
                    var type = condition.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (var action in rule.ActionGroup.Actions.First().AvailableActions)
                {
                    var type = action.GetType();
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
                var rule = new Rule();

                foreach (var trigger in rule.TriggerGroup.Triggers.First().AvailableTriggers)
                {
                    var type = trigger.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (var condition in rule.ConditionGroup.Conditions.First().AvailableConditions)
                {
                    var type = condition.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (var action in rule.ActionGroup.Actions.First().AvailableActions)
                {
                    var type = action.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }


                TextReader reader = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Rule>), ruleTypes.ToArray());
                var result = (ObservableCollection<Rule>)serializer.Deserialize(reader);
                reader.Close();
                return result;
            }
        }
    }
}
