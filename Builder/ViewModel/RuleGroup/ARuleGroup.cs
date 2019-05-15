using MQChatter.Common;
using MQChatter.ViewModel.RuleGroup.Action;
using MQChatter.ViewModel.RuleGroup.Condition;
using MQChatter.ViewModel.RuleGroup.Trigger;
using System;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup
{
    public class ARuleGroup : NotifyPropertyChangedBase
    {
        private string _groupName;

        public TriggerGroup TriggerGroup { get; set; }

        public ConditionGroup ConditionGroup { get; set; }

        public ActionGroup ActionGroup { get; set; }

        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
        }

        public string Title { get; set; }

        [XmlIgnore]
        public int ProcessCount { get; set; }

        public ARuleGroup()
        {
            GroupName = "<default group>";
            Title = "<default title>";
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
            foreach (ATrigger trigger in TriggerGroup.Triggers)
            {
                trigger.Selected.Reset();
            }

            foreach (ACondition condition in ConditionGroup.Conditions)
            {
                condition.Selected.Reset();
            }

            foreach (AAction action in ActionGroup.Actions)
            {
                action.Selected.Reset();
            }
        }
    }
}
