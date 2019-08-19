using MQChatter.ViewModel.RuleGroup.Action;
using System.Xml;

namespace MQChatter.Model.Action
{
    public class NoAction : ActionBase
    {
        public override string DisplayName => "No action";

        protected override bool ProcessAction(XmlDocument doc, int ruleProcessCount, ActionGroup actionGroup)
        {
            return true;
        }
    }
}