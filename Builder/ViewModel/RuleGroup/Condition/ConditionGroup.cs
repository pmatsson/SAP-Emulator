using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Condition
{
    public class ConditionGroup : NotifyPropertyChangedBase, IRuleUnit
    {
        private ObservableCollection<ACondition> _conditions;

        [XmlIgnore]
        public RelayCommand AddConditionCommand { get; private set; }

        [XmlIgnore]
        public RelayCommand<ACondition> RemoveConditionCommand { get; private set; }

        [XmlElement("ConditionItem")]
        public ObservableCollection<ACondition> Conditions
        {
            get => _conditions;
            set => SetProperty(ref _conditions, value);
        }

        public void AddCondition()
        {
            Conditions.Add(new ACondition());
        }

        public bool CanAddCondition()
        {
            return true;
        }

        public void RemoveCondition(ACondition condition)
        {
            Conditions.Remove(condition);
        }

        public bool CanRemoveCondition(ACondition condition)
        {
            return !condition.Equals(Conditions.First());
        }

        public ConditionGroup()
        {
            Conditions = new ObservableCollection<ACondition>();
            AddConditionCommand = new RelayCommand(AddCondition, CanAddCondition);
            RemoveConditionCommand = new RelayCommand<ACondition>(RemoveCondition, CanRemoveCondition);
        }
    }
}