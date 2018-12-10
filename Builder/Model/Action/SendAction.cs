using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [XmlType("Send")]
    public class SendAction : ActionBase
    {
        public override string DisplayName => "Send";

        [XmlAttribute("path")]
        public override string Param1 { get; set; }
    }
}
