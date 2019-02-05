using MQChatter.ViewModel.RuleGroup.Action;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MQChatter.Model.Action
{
    [XmlType("Add")]
    public class AddAction : ActionBase
    {
        //private string _filePath;
        private string _path;

        private int _value;

        public override string DisplayName => "Add";

        //[XmlElement("FilePath")]
        //public string FilePath
        //{
        //    get => _filePath;
        //    set
        //    {
        //        _filePath = value;
        //        SetProperty(ref _filePath, value);
        //    }
        //}

        [XmlElement("Path")]
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                SetProperty(ref _path, value);
            }
        }

        [XmlElement("Value")]
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                SetProperty(ref _value, value);
            }
        }

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
                throw new ArgumentException("The provided rule group does not contain any Send Actions");
            }

            try
            {
                string text = File.ReadAllText(filePath, Encoding.Default);

                XmlDocument addDoc = new XmlDocument();
                addDoc.LoadXml(text);

                foreach (XmlNode node in addDoc.SelectNodes(Path))
                {
                    if ((node.ChildNodes.Count == 1) && node.FirstChild is XmlText)
                    {
                        if (int.TryParse(node.FirstChild.InnerText, out int docValue))
                        {
                            docValue += Value;
                            node.FirstChild.InnerText = docValue.ToString();
                            addDoc.Save(filePath);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}