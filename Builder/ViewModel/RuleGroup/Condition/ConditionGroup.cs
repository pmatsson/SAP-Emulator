using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Condition
{
    public class ConditionGroup : NotifyPropertyChangedBase, IRuleUnit
    {
        private ObservableCollection<ACondition> _conditions;
        private string _errorMsg;
        private int _noErrors;

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
        public int NumberOfErrors
        {
            get => _noErrors;
            set => SetProperty(ref _noErrors, value);
        }

        public string ErrorMessage
        {
            get => _errorMsg;
            set => SetProperty(ref _errorMsg, value);
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

        public bool ValidateUnit()
        {
            NumberOfErrors = 0;
            ErrorMessage = "No errors in configuration :)";
            return true;
        }

        public ConditionGroup()
        {
            Conditions = new ObservableCollection<ACondition>();
            AddConditionCommand = new RelayCommand(AddCondition, CanAddCondition);
            RemoveConditionCommand = new RelayCommand<ACondition>(RemoveCondition, CanRemoveCondition);

            Conditions.CollectionChanged += Conditions_CollectionChanged;
        }

        private void Conditions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ACondition acondition = item as ACondition;
                    if (acondition != null && e.Action == NotifyCollectionChangedAction.Add)
                    {
                        acondition.PropertyChanged += Acondition_PropertyChanged; 
                        ValidateUnit();
                    }
                }
            }
            if (e?.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ACondition acondition = item as ACondition;
                    if (acondition != null && e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        acondition.PropertyChanged -= Acondition_PropertyChanged;
                        ValidateUnit();
                    }
                }
            }
        }

        private void Acondition_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
                ValidateUnit();
        }
    }
}