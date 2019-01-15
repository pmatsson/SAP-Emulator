using Builder.Common;
using Builder.Model.Action;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Builder.ViewModel
{

    public class Action : NotifyPropertyChangedBase
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
                    var loadedActions = new ObservableCollection<ActionBase>();
                    foreach (var action in AvailableActions)
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

        public Action()
        {
            AvailableActions = new ObservableCollection<ActionBase>()
            {
                new SendAction(),
                new AddAction()
            };

            if (Selected == null) Selected = AvailableActions.First();
        }
    }

    public class ActionGroup : NotifyPropertyChangedBase
    {

        private ObservableCollection<Action> _actions;

        [XmlIgnore]
        public RelayCommand AddActionCommand { get; private set; }

        [XmlIgnore]
        public RelayCommand<Action> RemoveActionCommand { get; private set; }

        [XmlElement("ActionItem")]
        public ObservableCollection<Action> Actions
        {
            get => _actions;
            set => SetProperty(ref _actions, value);
        }

        public void AddAction()
        {
            Actions.Add(new Action());
        }

        public bool CanAddAction()
        {
            return true;
        }

        public void RemoveAction(Action action)
        {
            Actions.Remove(action);
        }

        public bool CanRemoveAction(Action action)
        {
            return !action.Equals(Actions.First());
        }

        public ActionGroup()
        {
            this.Actions = new ObservableCollection<Action>();
            this.AddActionCommand = new RelayCommand(this.AddAction, this.CanAddAction);
            this.RemoveActionCommand = new RelayCommand<Action>(this.RemoveAction, this.CanRemoveAction);
        }

    }
}
