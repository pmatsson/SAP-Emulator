using Builder.Common;
using NLog;
using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [XmlType("Contains")]
    public class ContainsCondition : ConditionBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override string DisplayName => "Contains";

        [XmlElement("Path")]
        public string Path { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }

        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            logger.Debug("Searching for {0}", Path);
            var nodes = doc?.SelectNodes(Path, NamespaceManager.CreateNamespaceManager(doc));
            foreach(XmlNode node in nodes)
            {
                logger.Debug("Matched {0}", node.InnerText);
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
