using MQChatter.Common;
using MQChatter.ViewModel.RuleGroup;
using System.Xml;

namespace MQChatter.Model
{
    public abstract class UnitBase : NotifyPropertyChangedBase
    {
        public virtual void Reset()
        {
            UnitProcessCount = 0;
        }

        public bool TryProcess(XmlDocument doc, int ruleProcessCount, IRuleUnit ruleUnit)
        {
            UnitProcessCount++;
            return Process(doc, ruleProcessCount, ruleUnit);
        }

        protected abstract bool Process(XmlDocument doc, int ruleProcessCount, IRuleUnit ruleUnit);

        protected int UnitProcessCount { get; set; }
    }
}