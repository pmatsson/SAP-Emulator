using MQChatter.ViewModel.RuleGroup;
using MQChatter.ViewModel.RuleGroup.Trigger;
using System;
using System.Xml;

namespace MQChatter.Model.Trigger
{
    [Serializable]
    public abstract class TriggerBase : UnitBase
    {
        public abstract string DisplayName { get; }

        protected override bool Process(XmlDocument doc, int ruleProcessCount, IRuleGroup ruleGroup)
        {
            TriggerGroup triggerGroup = ruleGroup as TriggerGroup;

            if (triggerGroup == null)
            {
                throw new ArgumentException("Invalid rule group provided, must be of type TriggerGroup");
            }

            return ProcessTrigger(doc, ruleProcessCount, triggerGroup);
        }

        protected abstract bool ProcessTrigger(XmlDocument doc, int ruleProcessCount, TriggerGroup triggerGroup);
    }
}