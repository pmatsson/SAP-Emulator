using MQChatter.Common;
using MQChatter.ViewModel.RuleGroup.Action;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MQChatter.Model.Action
{
    public class CopyAction : ActionBase
    {
        public override string DisplayName => "Copy";

        public string PathFrom { get; set; }

        public string PathTo { get; set; }

        protected override bool ProcessAction(XmlDocument doc, int ruleProcessCount, ActionGroup actionGroup)
        {
            string filePath;

            SendAction sendAction = actionGroup
                ?.Actions.Where(x => x.Selected is SendAction)
                ?.First()
                ?.Selected as SendAction;

            if (sendAction != null)
            {
                filePath = sendAction.FilePath;
            }
            else
            {
                throw new ArgumentException("The provided rule group must contain a Send Action");
            }

            try
            {
                string text = File.ReadAllText(filePath, Encoding.Default);

                XmlDocument toDoc = new XmlDocument();
                toDoc.LoadXml(text);

                foreach (XmlNode nodeTo in toDoc.SelectNodes(PathTo, NamespaceManager.CreateNamespaceManager(toDoc)))
                {
                    if ((nodeTo.ChildNodes.Count == 1) && nodeTo.FirstChild is XmlText)
                    {
                        foreach (XmlNode nodeFrom in doc.SelectNodes(PathFrom, NamespaceManager.CreateNamespaceManager(doc)))
                        {
                            if ((nodeFrom.ChildNodes.Count == 1) && nodeFrom.FirstChild is XmlText)
                            {
                                nodeTo.FirstChild.InnerText = nodeFrom.FirstChild.InnerText;
                                toDoc.Save(filePath);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
    }
}