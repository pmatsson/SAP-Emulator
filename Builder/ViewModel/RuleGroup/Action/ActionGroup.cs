﻿using GalaSoft.MvvmLight.Command;
using MQChatter.Common;
using MQChatter.Model.Action;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;

namespace MQChatter.ViewModel.RuleGroup.Action
{
    public class ActionGroup : NotifyPropertyChangedBase, IRuleUnit
    {
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
            Actions.Add(new AAction());
        }

        public bool CanAddAction()
        {
            return true;
        }

        public void RemoveAction(AAction action)
        {
            Actions.Remove(action);
        }

        public bool CanRemoveAction(AAction action)
        {
            return !action.Equals(Actions.First());
        }

        public int ErrorsInConfiguration
        {
            get => _nofErrors;
            set => SetProperty(ref _nofErrors, value);
        }

        public bool ValidateUnit()
        {
            void AddError(string msg)
            {
                ErrorsInConfiguration++;
            }

            ErrorsInConfiguration = 0;

            AAction add = Actions.FirstOrDefault(x => x.Selected.GetType() == typeof(AddAction));
            AAction cpy = Actions.FirstOrDefault(x => x.Selected.GetType() == typeof(CopyAction));
            AAction snd = Actions.FirstOrDefault(x => x.Selected.GetType() == typeof(SendAction));
            ObservableCollection<AAction> ss = Actions;
            if (add != null && snd == null)
            {
                AddError("'Add' requires a 'Send' in the same group");
            }

            if (cpy != null && snd == null)
            {
                AddError("'Copy' requires a 'Send' in the same group and 'Recieved' in the same rule");
            }

            return ErrorsInConfiguration == 0;
        }

        public ActionGroup()
        {
            Actions = new ObservableCollection<AAction>();
            Actions.CollectionChanged += Actions_CollectionChanged;
            AddActionCommand = new RelayCommand(AddAction, CanAddAction);
            RemoveActionCommand = new RelayCommand<AAction>(RemoveAction, CanRemoveAction);
        }

        private void Actions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    AAction aaction = item as AAction;
                    if (aaction != null && e.Action == NotifyCollectionChangedAction.Add)
                    {
                        aaction.PropertyChanged += Aaction_PropertyChanged;
                        ValidateUnit();
                    }
                }
            }
            if (e?.OldItems != null)
            {
                foreach (object item in e.OldItems)
                {
                    AAction aaction = item as AAction;
                    if (aaction != null && e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        aaction.PropertyChanged -= Aaction_PropertyChanged;
                        ValidateUnit();
                    }
                }
            }
        }

        private void Aaction_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
            {
                ValidateUnit();
            }
        }
    }
}