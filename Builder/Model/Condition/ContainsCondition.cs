using MQChatter.Common;
using MQChatter.ViewModel.RuleGroup.Condition;
using NLog;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace MQChatter.Model.Condition
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

        protected override bool ProcessCondition(XmlDocument doc, int ruleProcessCount, ConditionGroup conditionGroup)
        {
            try
            {
                logger.Debug("Searching for {0}", Path);
                XmlNodeList nodes = doc?.SelectNodes(Path, NamespaceManager.CreateNamespaceManager(doc));
                foreach (XmlNode node in nodes)
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
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return false;
        }
    }
}