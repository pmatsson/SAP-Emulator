using MQChatter.ViewModel.RuleGroup.Condition;
using System.Xml;
using System.Xml.Serialization;

namespace MQChatter.Model.Condition
{
    [XmlType("Always")]
    public class AlwaysCondition : ConditionBase
    {
        public override string DisplayName => "Always";

        protected override bool ProcessCondition(XmlDocument doc, int ruleProcessCount, ConditionGroup conditionGroup)
        {
            return true;
        }
    }
}