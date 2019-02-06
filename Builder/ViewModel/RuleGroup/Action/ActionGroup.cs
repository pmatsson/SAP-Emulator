using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using MQChatter.Model.Action;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Action
{
    public class ActionGroup : NotifyPropertyChangedBase, IRuleUnit
    {
        private string _errorMessage;
        private int _nofErrors;

        private ObservableCollection<AAction> _actions;

        [XmlIgnore]
        public RelayCommand AddActionCommand { get; private set; }

        [XmlIgnore]
        public RelayCommand<AAction> RemoveActionCommand { get; private set; }

        [XmlElement("ActionItem")]
        public ObservableCollection<AAction> Actions
        {
            get => _actions;
            set => SetProperty(ref _actions, value);
        }

        public void AddAction()
        {
            var action = new AAction();
            action.PropertyChanged += Action_PropertyChanged;
            Actions.Add(action);
            ValidateGroup();
        }

        private void Action_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Selected")
                ValidateGroup();
        }

        public bool CanAddAction()
        {
            return true;
        }

        public void RemoveAction(AAction action)
        {
            Actions.Remove(action);
            action.PropertyChanged -= Action_PropertyChanged;
            ValidateGroup();
        }

        public bool CanRemoveAction(AAction action)
        {
            return !action.Equals(Actions.First());
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public int NumberOfErrors
        {
            get => _nofErrors;
            set => SetProperty(ref _nofErrors, value);
        }

        public bool ValidateGroup()
        {
            void AddError(string msg)
            {
                if (NumberOfErrors > 0)
                {
                    ErrorMessage += "\n";
                }
                else
                {
                    ErrorMessage = "";
                }

                NumberOfErrors++;
                ErrorMessage += NumberOfErrors.ToString() + ". " + msg + ".";
            }

            NumberOfErrors = 0;
            ErrorMessage = "No errors in configuration :)";

            var add = Actions.FirstOrDefault(x => x.Selected.GetType() == typeof(AddAction));
            var cpy = Actions.FirstOrDefault(x => x.Selected.GetType() == typeof(CopyAction));
            var snd = Actions.FirstOrDefault(x => x.Selected.GetType() == typeof(SendAction));

            if (add != null && snd == null)
            {
                AddError("'Add' requires a 'Send' in the same group");
            }

            if(cpy != null && snd == null)
            {
                AddError("'Copy' requires a 'Send' in the same group and 'Recieved' in the same rule");
            }

            return NumberOfErrors == 0;
        }

        public ActionGroup()
        {
            Actions = new ObservableCollection<AAction>();
            AddActionCommand = new RelayCommand(AddAction, CanAddAction);
            RemoveActionCommand = new RelayCommand<AAction>(RemoveAction, CanRemoveAction);
        }
    }
}