using System;
using System.Xml.Serialization;

namespace Builder.Model.Trigger
{
    [XmlType("Recieved")]
    public class ReceivedTrigger : TriggerBase
    {
        public override string DisplayName => "Received";

        [XmlElement("DocumentType")]
        public override string Param1 { get; set; }

        public string GetDocumentType() => Param1;

    }
}
