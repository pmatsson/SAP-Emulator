using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Action
{
    public class ActionGroup : NotifyPropertyChangedBase, IRuleGroup
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
            Actions = new ObservableCollection<Action>();
            AddActionCommand = new RelayCommand(AddAction, CanAddAction);
            RemoveActionCommand = new RelayCommand<Action>(RemoveAction, CanRemoveAction);
        }
    }
}