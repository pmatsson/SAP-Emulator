using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Trigger
{
    [XmlType("Once")]
    public class OnceTrigger : TriggerBase
    {
        public override string DisplayName => "Once";

        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            return ruleProcessCount == 0;
        }
    }
}
