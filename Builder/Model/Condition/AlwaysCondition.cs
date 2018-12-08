using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [XmlType("Always")]
    public class AlwaysCondition : ICondition
    {
        public override string DisplayName => "Always";
    }
}
