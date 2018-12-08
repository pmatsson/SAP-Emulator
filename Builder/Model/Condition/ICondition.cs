using System;
using System.Xml.Serialization;

namespace Builder.Model.Condition
{
    [Serializable]
    public abstract class ICondition
    {
        public abstract string DisplayName { get; }
    }
}
