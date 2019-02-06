using MQChatter.Common;
using MQChatter.ViewModel.RuleGroup.Action;
using MQChatter.ViewModel.RuleGroup.Condition;
using MQChatter.ViewModel.RuleGroup.Trigger;


namespace MQChatter.ViewModel.RuleGroup
{
    public class ARuleGroup : NotifyPropertyChangedBase
    {
        public TriggerGroup TriggerGroup { get; set; }

        public ConditionGroup ConditionGroup { get; set; }

        public ActionGroup ActionGroup { get; set; }

        public int ProcessCount { get; set; }

        public ARuleGroup()
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
