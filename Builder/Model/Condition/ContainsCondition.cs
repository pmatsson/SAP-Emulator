using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [XmlType("Contains")]
    public class ContainsCondition : ConditionBase
    {
        public override string DisplayName => "Contains";

        [XmlElement("Path")]
        public override string Param1 { get; set; }

        [XmlElement("Value")]
        public override string Param2 { get; set; }
    }
}
