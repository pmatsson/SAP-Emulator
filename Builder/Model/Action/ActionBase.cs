using MQChatter.ViewModel.RuleGroup;
using MQChatter.ViewModel.RuleGroup.Action;
using System;
using System.Xml;

namespace MQChatter.Model.Action
{
    [Serializable]
    public abstract class ActionBase : UnitBase
    {
        public abstract string DisplayName { get; }

        protected override bool Process(XmlDocument doc, int ruleProcessCount, IRuleGroup ruleGroup)
        {
            ActionGroup actionGroup = ruleGroup as ActionGroup;

            if (actionGroup == null)
            {
                throw new ArgumentException("Invalid rule group provided, must be of type ActionGroup");
            }

            return ProcessAction(doc, ruleProcessCount, actionGroup);
        }

        protected abstract bool ProcessAction(XmlDocument doc, int ruleProcessCount, ActionGroup actionGroup);
    }
}