using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Trigger
{
    public class TriggerGroup : NotifyPropertyChangedBase, IRuleUnit
    {
        private ObservableCollection<ATrigger> _triggers;
        private int _noErrors;

        [XmlIgnore]
        public RelayCommand AddTriggerCommand { get; private set; }

        [XmlIgnore]
        public RelayCommand<ATrigger> RemoveTriggerCommand { get; private set; }

        [XmlElement("TriggerItem")]
        public ObservableCollection<ATrigger> Triggers
        {
            get => _triggers;
            set => SetProperty(ref _triggers, value);
        }

        public int ErrorsInConfiguration
        {
            get => _noErrors;
            set => SetProperty(ref _noErrors, value);
        }

        public void AddTrigger()
        {
            Triggers.Add(new ATrigger());
        }

        public bool CanAddTrigger()
        {
            return true;
        }

        public void RemoveTrigger(ATrigger trigger)
        {
            Triggers.Remove(trigger);
        }

        public bool CanRemoveCondition(ATrigger trigger)
        {
            return !trigger.Equals(Triggers.First());
        }

        public bool ValidateUnit()
        {
            ErrorsInConfiguration = 0;
            return ErrorsInConfiguration == 0;
        }

        public TriggerGroup()
        {
            Triggers = new ObservableCollection<ATrigger>();
            AddTriggerCommand = new RelayCommand(AddTrigger, CanAddTrigger);
            RemoveTriggerCommand = new RelayCommand<ATrigger>(RemoveTrigger, CanRemoveCondition);
            Triggers.CollectionChanged += Triggers_CollectionChanged;
        }

        private void Triggers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ATrigger atrigger = item as ATrigger;
                    if (atrigger != null && e.Action == NotifyCollectionChangedAction.Add)
                    {
                        atrigger.PropertyChanged += Atrigger_PropertyChanged;
                        ValidateUnit();
                    }
                }
            }
            if (e?.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ATrigger atrigger = item as ATrigger;
                    if (atrigger != null && e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        atrigger.PropertyChanged -= Atrigger_PropertyChanged;
                        ValidateUnit();
                    }
                }
            }
        }

        private void Atrigger_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
                ValidateUnit();
        }
    }
}