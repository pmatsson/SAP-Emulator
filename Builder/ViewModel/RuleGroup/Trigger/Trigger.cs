using MQChatter.Common;
using MQChatter.Model.Trigger;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Trigger
{
    public class Trigger : NotifyPropertyChangedBase
    {
        private TriggerBase _selected;

        private ObservableCollection<TriggerBase> _availableTriggers;

        [XmlIgnore]
        public ObservableCollection<TriggerBase> AvailableTriggers
        {
            get => _availableTriggers;
            set => SetProperty(ref _availableTriggers, value);
        }

        [XmlElement("Trigger")]
        public TriggerBase Selected
        {
            get => _selected;
            set
            {
                // Deserialized values must be among the options for the combobox
                if (!AvailableTriggers.Contains(value))
                {
                    ObservableCollection<TriggerBase> loadedTriggers = new ObservableCollection<TriggerBase>();
                    foreach (TriggerBase trigger in AvailableTriggers)
                    {
                        if (value.GetType() == trigger.GetType())
                        {
                            loadedTriggers.Add(value);
                        }
                        else
                        {
                            loadedTriggers.Add(trigger);
                        }
                    }
                    AvailableTriggers = loadedTriggers;
                }
                SetProperty(ref _selected, value);
            }
        }

        public Trigger()
        {
            AvailableTriggers = new ObservableCollection<TriggerBase>()
            {
                new OnceTrigger(),
                new TimerTrigger(),
                new ReceivedTrigger()
            };

            if (Selected == null)
            {
                Selected = AvailableTriggers.First();
            }
        }
    }
}