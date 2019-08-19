using MQChatter.Common;
using MQChatter.Model.Condition;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Condition
{
    public class ACondition : NotifyPropertyChangedBase
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
                    ObservableCollection<ConditionBase> loadedConditions = new ObservableCollection<ConditionBase>();
                    foreach (ConditionBase condition in AvailableConditions)
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

        public ACondition()
        {
            AvailableConditions = new ObservableCollection<ConditionBase>()
            {
                new AlwaysCondition(),
                new ContainsCondition(),
                new CountCondition()
            };

            if (Selected == null)
            {
                Selected = AvailableConditions.First();
            }
        }
    }
}