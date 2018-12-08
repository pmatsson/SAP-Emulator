using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [XmlType("Contains")]
    public class ContainsCondition : ICondition
    {
        public override string DisplayName => "Contains";
    }
}
