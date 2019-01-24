using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    public class CountCondition : ConditionBase
    {
        public override string DisplayName => "Count";

        public string Operator { get; set; } = "=";

        public string Value { get; set; }

        public bool Resets { get; set; }


        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            int value;
            bool result;
            int.TryParse(Value, out value);
            switch (Operator)
            {
                case "=":
                    result = this.UnitProcessCount == value;
                    break;
                case "<":
                    result = this.UnitProcessCount < value;
                    break;
                case ">":
                    result = this.UnitProcessCount > value;
                    break;
                case "<=":
                    result = this.UnitProcessCount <= value;
                    break;
                case ">=":
                    result = this.UnitProcessCount >= value;
                    break;
                default:
                    result = false;
                    break;
            }

            if (result && Resets) UnitProcessCount = 0;

            return result;
        }
    }
}
