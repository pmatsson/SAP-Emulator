using System.ComponentModel;
using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [XmlType("Send")]
    public class SendAction : ActionBase
    {
        private string _param1;

        public override string DisplayName => "Send";

        [XmlElement("FilePath")]
        public override string Param1
        {
            get => _param1;
            set
            {
                _param1 = value;
                SetProperty(ref _param1, value);
            }
        }

        public string GetFilePath() => Param1;

    }
}
