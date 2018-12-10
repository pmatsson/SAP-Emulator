using Builder.Model.Action;
using Builder.Model.Condition;
using Builder.Model.Trigger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Builder.ViewModel
{

    public class TriggerGroup : ViewModelBase
    {
        private TriggerBase _selected;

        private ObservableCollection<TriggerBase> _triggers;

        [XmlIgnore]
        public ObservableCollection<TriggerBase> Triggers
        {
            get => _triggers;
            set => SetProperty(ref _triggers, value);
        }

        [XmlElement("Trigger")]
        public TriggerBase Selected
        {
            get => _selected;
            set
            {
                // Deserialized values must be among the options for the combobox
                if (!Triggers.Contains(value))
                {
                    var loadedTriggers = new ObservableCollection<TriggerBase>();
                    foreach (var trigger in Triggers)
                    {
                        if (value.GetType() == trigger.GetType())
                        {
                            loadedTriggers.Add(value);
                        }
                        else
                        {
                            loadedTriggers.Add(trigger);
                        }
                    }
                    Triggers = loadedTriggers;
                }
                SetProperty(ref _selected, value);
            }
        }


        public TriggerGroup()
        {
            Triggers = new ObservableCollection<TriggerBase>()
            {
                new OnceTrigger(),
                new ReceivedTrigger()
            };

            if (Selected == null) Selected = Triggers.First();
        }
    }

    public class ConditionGroup : ViewModelBase
    {
        private ConditionBase _selected;

        private ObservableCollection<ConditionBase> _conditions;

        [XmlIgnore]
        public ObservableCollection<ConditionBase> Conditions
        {
            get => _conditions;
            set => SetProperty(ref _conditions, value);
        }

        [XmlElement("Condition")]
        public ConditionBase Selected
        {
            get => _selected;
            set
            {
                // Deserialized values must be among the options for the combobox
                if (!Conditions.Contains(value))
                {
                    var loadedConditions = new ObservableCollection<ConditionBase>();
                    foreach (var condition in Conditions)
                    {
                        if (value.GetType() == condition.GetType())
                        {
                            loadedConditions.Add(value);
                        }
                        else
                        {
                            loadedConditions.Add(condition);
                        }
                    }
                    Conditions = loadedConditions;
                }
                SetProperty(ref _selected, value);
            }
        }

        public ConditionGroup()
        {
            Conditions = new ObservableCollection<ConditionBase>()
            {
                new AlwaysCondition(),
                new ContainsCondition()
            };

            if (Selected == null) Selected = Conditions.First();
        }
    }

    public class ActionGroup : ViewModelBase
    {
        private ActionBase _selected;

        private ObservableCollection<ActionBase> _actions;

        [XmlIgnore]
        public ObservableCollection<ActionBase> Actions
        {
            get => _actions;
            set => SetProperty(ref _actions, value);
        }

        [XmlElement("Action")]
        public ActionBase Selected
        {
            get => _selected;
            set
            {
                // Deserialized values must be among the options for the combobox
                if (!Actions.Contains(value))
                {
                    var loadedActions = new ObservableCollection<ActionBase>();
                    foreach (var action in Actions)
                    {
                        if (value.GetType() == action.GetType())
                        {
                            loadedActions.Add(value);
                        }
                        else
                        {
                            loadedActions.Add(action);
                        }
                    }
                    Actions = loadedActions;
                }
                SetProperty(ref _selected, value);
            }
        }

        public ActionGroup()
        {
            Actions = new ObservableCollection<ActionBase>()
            {
                new SendAction()
            };

            if (Selected == null) Selected = Actions.First();
        }
    }

    public class Rule
    {
        public TriggerGroup TriggerGroup { get; set; }

        public ConditionGroup ConditionGroup { get; set; }

        public ActionGroup ActionGroup { get; set; }

        public Rule()
        {
            TriggerGroup = new TriggerGroup();
            ConditionGroup = new ConditionGroup();
            ActionGroup = new ActionGroup();
        }

    }

    public class ContainerViewModel : ViewModelBase
    {
        private ObservableCollection<Rule> _rules;

        public ObservableCollection<Rule> MyRows
        {
            get => _rules;
            set => SetProperty(ref _rules, value);
        }

        public ContainerViewModel()
        {
            MyRows = new ObservableCollection<Rule>();

        }

        public void AddRow()
        {
            MyRows.Add(new Rule());
        }

        public void SerializeRules()
        {
            TextWriter writer = new StreamWriter("test.xml");
            RuleSerializer.Serialize(MyRows.ToList(), writer);
            //TextWriter writer = new StreamWriter(filename);


        }

        public void DeSerializeRules()
        {
            MyRows = RuleSerializer.DeSerialize("test.xml");
        }

        public static class RuleSerializer
        {
            public static void Serialize(List<Rule> rules, TextWriter stream)
            {
                List<Type> ruleTypes = new List<Type>();
                foreach (Rule rule in rules)
                {
                    Type type = rule.TriggerGroup.Selected.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }

                    type = rule.ConditionGroup.Selected.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }

                    type = rule.ActionGroup.Selected.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }
                XmlSerializer serializer = new XmlSerializer(typeof(List<Rule>), ruleTypes.ToArray());
                serializer.Serialize(stream, rules);
            }

            public static ObservableCollection<Rule> DeSerialize(string path)
            {
                List<Type> ruleTypes = new List<Type>();
                var rule = new Rule();

                foreach (var trigger in rule.TriggerGroup.Triggers)
                {
                    var type = trigger.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (var condition in rule.ConditionGroup.Conditions)
                {
                    var type = condition.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (var action in rule.ActionGroup.Actions)
                {
                    var type = action.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }


                TextReader reader = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Rule>), ruleTypes.ToArray());
                return (ObservableCollection<Rule>)serializer.Deserialize(reader);
            }
        }
    }
}
