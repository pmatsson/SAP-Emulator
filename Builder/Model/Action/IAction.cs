using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Builder.Model.Action
{
    [Serializable]
    public abstract class IAction
    {
        public abstract string DisplayName { get; }
    }
}
