using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [XmlType("Send")]
    public class SendAction : ActionBase
    {
        public override string DisplayName => "Send";

        [XmlElement("FilePath")]
        public override string Param1 { get; set; }


        public string GetFilePath() => Param1;

    }
}
