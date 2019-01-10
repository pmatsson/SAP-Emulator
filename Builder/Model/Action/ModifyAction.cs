using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [XmlType("Modify")]
    public class ModifyAction : ActionBase
    {
        private string _param1;
        private string _param2;

        public override string DisplayName => "Modify";

        [XmlElement("Operator")]
        public override string Param1
        {
            get => _param1;
            set
            {
                _param1 = value;
                SetProperty(ref _param1, value);
            }
        }

        [XmlElement("Value")]
        public override string Param2
        {
            get => _param2;
            set
            {
                _param2 = value;
                SetProperty(ref _param2, value);
            }
        }

        public string GetOperator() => Param1;

    }
}
