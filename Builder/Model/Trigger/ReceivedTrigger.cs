using System.Xml.Serialization;

namespace Builder.Model.Trigger
{
    [XmlType("Recieved")]
    public class ReceivedTrigger : ITrigger
    {
        public override string DisplayName => "Received";

        [XmlAttribute("telegramType")]
        public override string Param1 { get; set; }
    }
}
