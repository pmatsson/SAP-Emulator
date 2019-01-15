using Builder.MQ;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Trigger
{
    [XmlType("Recieved")]
    public class ReceivedTrigger : TriggerBase
    {
        private MQProps _mqSettings;
        private MQHandler _mqHandler;

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
            _mqHandler = new MQHandler();
        }

        public bool Process(out XmlDocument doc, int ruleProcessCount)
        {
            doc = null;
            if (!_mqHandler.IsConnected())
            {
                if (!_mqHandler.Connect(MQSettings))
                {
                    return false;
                }
            }

            string msg = "";
            doc = new XmlDocument();
            if (_mqHandler.Read(ref msg))
            {
                try
                {
                    doc.LoadXml(msg);
                }
                catch (XmlException ex)
                {
                    return false;
                }
            }

            return DocumentType == doc?.DocumentElement?.Name;
        }

        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            base.Reset();
            _mqHandler.Disconnect();
        }
    }
}
