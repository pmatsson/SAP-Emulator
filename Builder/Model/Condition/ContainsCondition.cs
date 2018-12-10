using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [XmlType("Contains")]
    public class ContainsCondition : ConditionBase
    {
        public override string DisplayName => "Contains";

        [XmlAttribute("value")]
        public override string Param1 { get; set; }
    }
}
