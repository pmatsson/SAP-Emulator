using Builder.MQ;
using NLog;
using System;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [XmlType("Send")]
    public class SendAction : ActionBase
    {
        private string _filePath;
        private MQProps _mqSettings;
        private MQHandler _mqHandler;

        public override string DisplayName => "Send";

        [XmlElement("FilePath")]
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                SetProperty(ref _filePath, value);
            }
        }

        public MQProps MQSettings
        {
            get => _mqSettings;
            set => SetProperty(ref _mqSettings, value);
        }

        public SendAction()
        {
            MQSettings = new MQProps();
            _mqHandler = new MQHandler();
        }

        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            if(!_mqHandler.IsConnected())
            {
                if (!_mqHandler.Connect(MQSettings))
                {
                    return false;
                }
            }

            bool result = _mqHandler.Write(File.ReadAllText(FilePath));
            _mqHandler.Disconnect();
            return result;
        }

        public override void Reset()
        {
            base.Reset();
            _mqHandler.Disconnect();
        }

    }
}
