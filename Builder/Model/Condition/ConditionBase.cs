using System;
using System.Xml;
using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [Serializable]
    public abstract class ConditionBase : UnitBase
    {
        public abstract string DisplayName { get; }

    }
}
