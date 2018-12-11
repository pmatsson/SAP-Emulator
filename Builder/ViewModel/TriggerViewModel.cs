using Builder.Model.Trigger;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Builder.ViewModel
{

    public class Trigger : ViewModelBase
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
                    var loadedTriggers = new ObservableCollection<TriggerBase>();
                    foreach (var trigger in AvailableTriggers)
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
                new ReceivedTrigger()
            };

            if (Selected == null) Selected = AvailableTriggers.First();
        }
    }

    public class TriggerGroup : ViewModelBase
    {
        private ObservableCollection<Trigger> _triggers;

        [XmlIgnore]
        public RelayCommand AddTriggerCommand { get; private set; }

        [XmlIgnore]
        public RelayCommand<Trigger> RemoveTriggerCommand { get; private set; }

        [XmlElement("Trigger")]
        public ObservableCollection<Trigger> Triggers
        {
            get => _triggers;
            set => SetProperty(ref _triggers, value);
        }

        public void AddTrigger()
        {
            Triggers.Add(new Trigger());
        }

        public bool CanAddTrigger()
        {
            return true;
        }

        public void RemoveTrigger(Trigger trigger)
        {
            Triggers.Remove(trigger);
        }

        public bool CanRemoveCondition(Trigger trigger)
        {
            return !trigger.Equals(Triggers.First());
        }

        public TriggerGroup()
        {
            this.Triggers = new ObservableCollection<Trigger>();
            this.AddTriggerCommand = new RelayCommand(this.AddTrigger, this.CanAddTrigger);
            this.RemoveTriggerCommand = new RelayCommand<Trigger>(this.RemoveTrigger, this.CanRemoveCondition);
        }
    }
}
