using MQChatter.ViewModel.RuleGroup;
using MQChatter.ViewModel.RuleGroup.Condition;
using System;
using System.Xml;

namespace MQChatter.Model.Condition
{
    [Serializable]
    public abstract class ConditionBase : UnitBase
    {
        public abstract string DisplayName { get; }

        protected override bool Process(XmlDocument doc, int ruleProcessCount, IRuleGroup ruleGroup)
        {
            ConditionGroup conditionGroup = ruleGroup as ConditionGroup;

            if (conditionGroup == null)
            {
                throw new ArgumentException("Invalid rule group provided, must be of type ConditionGroup");
            }

            return ProcessCondition(doc, ruleProcessCount, conditionGroup);
        }

        protected abstract bool ProcessCondition(XmlDocument doc, int ruleProcessCount, ConditionGroup conditionGroup);
    }
}