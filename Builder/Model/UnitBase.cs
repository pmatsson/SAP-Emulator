using Builder.Common;
using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model
{
    public abstract class UnitBase : NotifyPropertyChangedBase
    {
        public virtual void Reset()
        {
            UnitProcessCount = 0;
        }

        public bool TryProcess(XmlDocument doc, int ruleProcessCount)
        {
            UnitProcessCount++;
            return Process(doc, ruleProcessCount);
        }

        protected abstract bool Process(XmlDocument doc, int ruleProcessCount);


        protected int UnitProcessCount { get; set; }
    }
}
