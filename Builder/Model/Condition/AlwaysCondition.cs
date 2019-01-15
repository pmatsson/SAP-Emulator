using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [XmlType("Always")]
    public class AlwaysCondition : ConditionBase
    {
        public override string DisplayName => "Always";

        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            return true;
        }
    }
}
