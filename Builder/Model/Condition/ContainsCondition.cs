using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [XmlType("Contains")]
    public class ContainsCondition : ConditionBase
    {
        public override string DisplayName => "Contains";

        [XmlElement("Path")]
        public string Path { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }

        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            var nodes = doc?.SelectNodes(Path);
            foreach(XmlNode node in nodes)
            {
                if (node.ChildNodes.Count == 1 && (node.FirstChild is XmlText))
                {
                    if (node.InnerText == Value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
