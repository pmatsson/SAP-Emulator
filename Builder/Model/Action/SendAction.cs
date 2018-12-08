using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [XmlType("Send")]
    public class SendAction : IAction
    {
        public override string DisplayName => "Send";
    }
}
