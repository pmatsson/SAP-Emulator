using Builder.Common;
using Builder.Model.Action;
using Builder.Model.Condition;
using Builder.Model.Trigger;
using Builder.MQ;
using Builder.Processor;
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

    public class Rule : NotifyPropertyChangedBase
    {
        public TriggerGroup TriggerGroup { get; set; }

        public ConditionGroup ConditionGroup { get; set; }


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

    public class RuleViewModel : NotifyPropertyChangedBase
    {
        private ObservableCollection<Rule> _rules;
        private ObservableCollection<Rule> _processedRules;
        private RuleProcessor _ruleProcessor;
        private bool _isRunningEmulator;
        private string _openDocument;

        public ObservableCollection<Rule> Rules
        {
            get => _rules;
            set => SetProperty(ref _rules, value);
        }

        public ObservableCollection<Rule> ProcessedRules
        {
            get => _processedRules;
            set => SetProperty(ref _processedRules, value);
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
            ProcessedRules = new ObservableCollection<Rule>();
            _isRunningEmulator = false;

        }

        public void StartEmulation()
        {
            var mqHandler = new MQHandler();
            if(IsRunningEmulator = mqHandler.Connect("QM1_DEV", "Q1_DEV", "SCC1", "localhost", 1414))
            { 
                _ruleProcessor = new RuleProcessor(Rules.ToList(), mqHandler);
                _ruleProcessor.RuleProcessed += RuleProcessor_RuleProcessed;
                var res = _ruleProcessor.Start();
            }
        }

        public void CancelEmulation()
        {
            if(_ruleProcessor != null)
            {
                _ruleProcessor.Cancel();
                ProcessedRules.Clear();
                IsRunningEmulator = false;
            }
        }

        private void RuleProcessor_RuleProcessed(object sender, Rule e)
        {
            if(!_processedRules.Contains(e))
                _processedRules.Add(e);
        }

        public void CreateRule()
        {
            Rules.Add(new Rule());
        }

        public void RemoveRule(Rule rule)
        {
            Rules.Remove(rule);
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


                ObservableCollection<Rule> result = null;
                TextReader reader = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Rule>), ruleTypes.ToArray());
                try
                {
                    result = (ObservableCollection<Rule>)serializer.Deserialize(reader);

                }
                catch(InvalidOperationException ex)
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
