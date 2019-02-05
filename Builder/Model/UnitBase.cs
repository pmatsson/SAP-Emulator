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

        public bool TryProcess(XmlDocument doc, int ruleProcessCount, IRuleGroup ruleGroup)
        {
            UnitProcessCount++;
            return Process(doc, ruleProcessCount, ruleGroup);
        }

        protected abstract bool Process(XmlDocument doc, int ruleProcessCount, IRuleGroup ruleGroup);

        protected int UnitProcessCount { get; set; }
    }
}