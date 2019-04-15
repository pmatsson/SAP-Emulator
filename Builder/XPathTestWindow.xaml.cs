using MQChatter.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace MQChatter
{
    /// <summary>
    /// Interaction logic for XPathTestWindow.xaml
    /// </summary>
    public partial class XPathTestWindow : Window, INotifyPropertyChanged
    {
        private string _xpath;
        private string _filepath;
        private string _result;

        public string XPath
        {
            get => _xpath;
            set
            {
                _xpath = value;
                OnPropertyChanged("XPath");
            }
        }

        public string FilePath
        {
            get => _filepath;
            set
            {
                _filepath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public XPathTestWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Result = "";
            XmlDocument doc;

            try
            {
                string xmlString = File.ReadAllText(FilePath, Encoding.Default);
                doc = new XmlDocument();
                doc.LoadXml(xmlString);
            }
            catch (Exception ex)
            {
                Result = ex.Message;
                return;
            }

            try
            {
                foreach (XmlNode node in doc.SelectNodes(XPath, NamespaceManager.CreateNamespaceManager(doc)))
                {
                    if ((node.ChildNodes.Count == 1) && node.FirstChild is XmlText)
                    {
                        Result = node.Name + " ==> " + node.FirstChild.InnerText.ToString();
                    }
                    else if (node.ChildNodes.Count > 1)
                    {
                        Result = "Ambiguous result, node has multiple children";
                    }
                    else if (node.ChildNodes.Count == 0)
                    {
                        Result = "Node has no children";
                    }
                }
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }


            if (Result == "") Result = "No match :(";
        }
    }
}
