using MQChatter.ViewModel.RuleGroup.Condition;
using System.Xml;

namespace MQChatter.Model.Condition
{
    public class CountCondition : ConditionBase
    {
        public override string DisplayName => "Count";

        public string Operator { get; set; } = "=";

        public string Value { get; set; }

        public bool Resets { get; set; }

        protected override bool ProcessCondition(XmlDocument doc, int ruleProcessCount, ConditionGroup conditionGroup)
        {
            bool result;
            int.TryParse(Value, out int value);
            switch (Operator)
            {
                case "=":
                    result = UnitProcessCount == value;
                    break;

                case "<":
                    result = UnitProcessCount < value;
                    break;

                case ">":
                    result = UnitProcessCount > value;
                    break;

                case "<=":
                    result = UnitProcessCount <= value;
                    break;

                case ">=":
                    result = UnitProcessCount >= value;
                    break;

                default:
                    result = false;
                    break;
            }

            if (result && Resets)
            {
                UnitProcessCount = 0;
            }

            return result;
        }
    }
}