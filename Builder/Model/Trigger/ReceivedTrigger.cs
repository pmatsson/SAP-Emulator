using MQChatter.MQ;
using MQChatter.ViewModel.RuleGroup.Trigger;
using System.Xml;
using System.Xml.Serialization;

namespace MQChatter.Model.Trigger
{
    [XmlType("Recieved")]
    public class ReceivedTrigger : TriggerBase
    {
        private MQProps _mqSettings;

        public override string DisplayName => "Received";

        [XmlElement("DocumentType")]
        public string DocumentType { get; set; }

        public MQProps MQSettings
        {
            get => _mqSettings;
            set => SetProperty(ref _mqSettings, value);
        }

        public ReceivedTrigger()
        {
            _mqSettings = new MQProps();
        }

        protected override bool ProcessTrigger(XmlDocument doc, int ruleProcessCount, TriggerGroup triggerGroup)
        {
            return DocumentType == doc?.DocumentElement?.Name;
        }
    }
}