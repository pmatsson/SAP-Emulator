using Builder.Model.Condition;
using Builder.Model.Trigger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder.ViewModel
{

    public class TriggerGroup : ViewModelBase
    {
        private ITrigger _selected;

        public ObservableCollection<ITrigger> Triggers { get; private set; }

        public ITrigger Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public TriggerGroup()
        {
            Triggers = new ObservableCollection<ITrigger>()
            {
                new OnceTrigger(),
                new ReceivedTrigger()
            };

            Selected = Triggers.First();
        }
    }

    public class ConditionGroup : ViewModelBase
    {
        private ICondition _selected;

        public ObservableCollection<ICondition> Conditions { get; private set; }

        public ICondition Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ConditionGroup()
        {
            Conditions = new ObservableCollection<ICondition>()
            {
                new AlwaysCondition(),
                new ContainsCondition()
            };

            Selected = Conditions.First();
        }
    }

    public class TestRow
    {
        public TriggerGroup TriggerCell { get; private set; }

        public ConditionGroup ConditionCell { get; private set; }


        public TestRow()
        {
            TriggerCell = new TriggerGroup();
            ConditionCell = new ConditionGroup();
        }

    }
    public class ContainerViewModel : ViewModelBase
    {

        public ObservableCollection<TestRow> MyRows { get; set; }

        public ContainerViewModel()
        {
            MyRows = new ObservableCollection<TestRow>();

        }

        public void AddRow()
        {
            MyRows.Add(new TestRow());
        }
    }
}
