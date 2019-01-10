using Builder.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [Serializable]
    public abstract class ActionBase :  NotifyPropertyChangedBase, IRule
    {
        public abstract string DisplayName { get; }

        [XmlIgnore]
        public virtual string Param1 { get; set; }

        [XmlIgnore]
        public virtual string Param2 { get; set; }

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
