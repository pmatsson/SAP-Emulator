using System.Xml.Serialization;

namespace Builder.Model.Trigger
{
    [XmlType("Once")]
    public class OnceTrigger : ITrigger
    {
        public override string DisplayName => "Once";
    }
}
