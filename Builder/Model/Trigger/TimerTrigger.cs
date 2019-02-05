using MQChatter.ViewModel.RuleGroup.Trigger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MQChatter.Model.Trigger
{
    public enum TimeUnit
    {
        Seconds,
        Minutes,
        Hours,
        Days
    }

    public class TimerTrigger : TriggerBase
    {
        public override string DisplayName => "Every";

        public int Time { get; set; }

        public TimeUnit TimeUnit { get; set; }

        public IList<TimeUnit> TimeUnits => Enum.GetValues(typeof(TimeUnit)).Cast<TimeUnit>().ToList();

        private DateTime _startTime;

        public override void Reset()
        {
            base.Reset();
            _startTime = DateTime.Now;
        }

        protected override bool ProcessTrigger(XmlDocument doc, int ruleProcessCount, TriggerGroup triggerGroup)
        {
            bool result = false;

            switch (TimeUnit)
            {
                case TimeUnit.Seconds:
                    result = (DateTime.Now - _startTime).Seconds >= Time;
                    break;

                case TimeUnit.Minutes:
                    result = (DateTime.Now - _startTime).Minutes >= Time;
                    break;

                case TimeUnit.Hours:
                    result = (DateTime.Now - _startTime).Hours >= Time;
                    break;

                case TimeUnit.Days:
                    result = (DateTime.Now - _startTime).Days >= Time;
                    break;
            }

            if (result)
            {
                Reset();
            }

            return result;
        }
    }
}