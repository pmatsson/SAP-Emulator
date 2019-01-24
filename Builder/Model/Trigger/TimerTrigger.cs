using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Builder.Model.Trigger
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


        protected override bool Process(XmlDocument doc, int ruleProcessCount)
        {
            bool result = false;

            switch(TimeUnit)
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
            
            if (result) Reset();
            return result;
        }

        public override void Reset()
        {
            base.Reset();
            _startTime = DateTime.Now;
        }
    }
}
