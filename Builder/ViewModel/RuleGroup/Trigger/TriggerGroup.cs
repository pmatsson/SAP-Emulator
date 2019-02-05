using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Trigger
{
    public class TriggerGroup : NotifyPropertyChangedBase, IRuleGroup
    {
        private ObservableCollection<Trigger> _triggers;

        [XmlIgnore]
        public RelayCommand AddTriggerCommand { get; private set; }

        [XmlIgnore]
        public RelayCommand<Trigger> RemoveTriggerCommand { get; private set; }

        [XmlElement("TriggerItem")]
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
            Triggers = new ObservableCollection<Trigger>();
            AddTriggerCommand = new RelayCommand(AddTrigger, CanAddTrigger);
            RemoveTriggerCommand = new RelayCommand<Trigger>(RemoveTrigger, CanRemoveCondition);
        }
    }
}