using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Trigger
{
    public class TriggerGroup : NotifyPropertyChangedBase, IRuleUnit
    {
        private ObservableCollection<ATrigger> _triggers;

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



        public TriggerGroup()
        {
            Triggers = new ObservableCollection<ATrigger>();
            AddTriggerCommand = new RelayCommand(AddTrigger, CanAddTrigger);
            RemoveTriggerCommand = new RelayCommand<ATrigger>(RemoveTrigger, CanRemoveCondition);
        }
    }
}