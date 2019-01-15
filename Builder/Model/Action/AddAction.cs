using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [XmlType("Add")]
    public class AddAction : ActionBase
    {
        private string _filePath;
        private string _path;
        private int _value;

        public override string DisplayName => "Add";

        [XmlElement("FilePath")]
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                SetProperty(ref _filePath, value);
            }
        }

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

        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            try
            {
                var text = File.ReadAllText(FilePath, Encoding.Default);

                XmlDocument addDoc = new XmlDocument();
                addDoc.LoadXml(text);

                foreach (XmlNode node in addDoc.SelectNodes(Path))
                {
                    if ((node.ChildNodes.Count == 1) && node.FirstChild is XmlText)
                    {
                        int docValue;
                        if (Int32.TryParse(node.FirstChild.InnerText, out docValue))
                        {
                            docValue += Value;
                            node.FirstChild.InnerText = docValue.ToString();
                            addDoc.Save(FilePath);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
