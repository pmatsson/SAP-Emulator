using Builder.Common;
using Builder.Model.Condition;
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
    public class Condition : NotifyPropertyChangedBase
    {
        private ConditionBase _selected;

        private ObservableCollection<ConditionBase> _availableConditions;


        [XmlIgnore]
        public ObservableCollection<ConditionBase> AvailableConditions
        {
            get => _availableConditions;
            set => SetProperty(ref _availableConditions, value);
        }

        [XmlElement("Condition")]
        public ConditionBase Selected
        {
            get => _selected;
            set
            {
                // Deserialized values must be among the options for the combobox
                if (!AvailableConditions.Contains(value))
                {
                    var loadedConditions = new ObservableCollection<ConditionBase>();
                    foreach (var condition in AvailableConditions)
                    {
                        if (value.GetType() == condition.GetType())
                        {
                            loadedConditions.Add(value);
                        }
                        else
                        {
                            loadedConditions.Add(condition);
                        }
                    }
                    AvailableConditions = loadedConditions;
                }
                SetProperty(ref _selected, value);
            }
        }

        public Condition()
        {
            AvailableConditions = new ObservableCollection<ConditionBase>()
            {
                new AlwaysCondition(),
                new ContainsCondition(),
                new CountCondition()
            };

            if (Selected == null) Selected = AvailableConditions.First();
        }
    }

    public class ConditionGroup : NotifyPropertyChangedBase
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
            this.Conditions = new ObservableCollection<Condition>();
            this.AddConditionCommand = new RelayCommand(this.AddCondition, this.CanAddCondition);
            this.RemoveConditionCommand = new RelayCommand<Condition>(this.RemoveCondition, this.CanRemoveCondition);
        }
    }
}
