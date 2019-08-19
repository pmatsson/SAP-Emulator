using MQChatter.Common;
using MQChatter.Model.Action;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Action
{
    public class AAction : NotifyPropertyChangedBase
    {
        private ActionBase _selected;

        private ObservableCollection<ActionBase> _availableActions;

        [XmlIgnore]
        public ObservableCollection<ActionBase> AvailableActions
        {
            get => _availableActions;
            set => SetProperty(ref _availableActions, value);
        }

        [XmlElement("Action")]
        public ActionBase Selected
        {
            get => _selected;
            set
            {
                // Deserialized values must be among the options for the combobox
                if (!AvailableActions.Contains(value))
                {
                    ObservableCollection<ActionBase> loadedActions = new ObservableCollection<ActionBase>();
                    foreach (ActionBase action in AvailableActions)
                    {
                        if (value.GetType() == action.GetType())
                        {
                            loadedActions.Add(value);
                        }
                        else
                        {
                            loadedActions.Add(action);
                        }
                    }
                    AvailableActions = loadedActions;
                }
                SetProperty(ref _selected, value);
            }
        }

        public AAction()
        {
            AvailableActions = new ObservableCollection<ActionBase>()
            {
                new SendAction(),
                new AddAction(),
                new CopyAction(),
                new NoAction(),
            };

            if (Selected == null)
            {
                Selected = AvailableActions.First();
            }
        }
    }
}