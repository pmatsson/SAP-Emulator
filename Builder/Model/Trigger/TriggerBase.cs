using System;
using System.Xml.Serialization;

namespace Builder.Model.Trigger
{
    [Serializable]
    public abstract class TriggerBase : IRule
    {
        public abstract string DisplayName { get; }

        [XmlIgnore]
        public virtual string Param1 { get; set; }

        [XmlIgnore]
        public virtual string Param2{ get; set; }

        public virtual bool ShouldSerializeParam1()
        {
            return Param1 != null;
        }

        public virtual bool ShouldSerializeParam2()
        {
            return Param2 != null;
        }
    }
}
