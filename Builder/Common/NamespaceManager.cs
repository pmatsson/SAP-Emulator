using System;
using System.Xml;
using System.Xml.XPath;

namespace MQChatter.Common
{
    public static class NamespaceManager
    {
        public static XmlNamespaceManager CreateNamespaceManager(XmlDocument Doc)
        {
            XPathNavigator Nav = Doc.SelectSingleNode("/*").CreateNavigator();
            XmlNamespaceManager Result = null;

            if (Nav.MoveToFirstNamespace())
            {
                Result = new XmlNamespaceManager(Doc.NameTable);

                do
                {
                    Result.AddNamespace(String.IsNullOrEmpty(Nav.Name) ? "default" : Nav.Name, Nav.Value);
                } while (Nav.MoveToNextNamespace());
            }

            return Result;
        }
    }
}