using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Condition
{
    public class ConditionGroup : NotifyPropertyChangedBase, IRuleGroup
    {
        private ObservableCollection<Condition> _conditions;

        [XmlIgnore]
        public RelayCommand AddConditionCommand { get; private set; }

        [XmlIgnore]
        public RelayCommand<Condition> RemoveConditionCommand { get; private set; }

        [XmlElement("ConditionItem")]
        public ObservableCollection<Condition> Conditions
        {
            get => _conditions;
            set => SetProperty(ref _conditions, value);
        }

        public void AddCondition()
        {
            Conditions.Add(new Condition());
        }

        public bool CanAddCondition()
        {
            return true;
        }

        public void RemoveCondition(Condition condition)
        {
            Conditions.Remove(condition);
        }

        public bool CanRemoveCondition(Condition condition)
        {
            return !condition.Equals(Conditions.First());
        }

        public ConditionGroup()
        {
            Conditions = new ObservableCollection<Condition>();
            AddConditionCommand = new RelayCommand(AddCondition, CanAddCondition);
            RemoveConditionCommand = new RelayCommand<Condition>(RemoveCondition, CanRemoveCondition);
        }
    }
}