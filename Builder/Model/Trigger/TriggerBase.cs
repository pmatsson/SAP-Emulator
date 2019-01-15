using System;
using System.Xml.Serialization;

namespace Builder.Model.Trigger
{
    [Serializable]
    public abstract class TriggerBase : UnitBase
    {
        public abstract string DisplayName { get; }

    }
}
