using System.Xml.Serialization;

namespace Builder.Model.Trigger
{
    [XmlType("Once")]
    public class OnceTrigger : TriggerBase
    {
        public override string DisplayName => "Once";

        [XmlIgnore]
        public int HitCount { get; set; }
    }
}
