using MQChatter.ViewModel.RuleGroup.Trigger;
using System.Xml;
using System.Xml.Serialization;

namespace MQChatter.Model.Trigger
{
    [XmlType("Once")]
    public class OnceTrigger : TriggerBase
    {
        public override string DisplayName => "Once";

        protected override bool ProcessTrigger(XmlDocument doc, int ruleProcessCount, TriggerGroup triggerGroup)
        {
            return ruleProcessCount == 0;
        }
    }
}