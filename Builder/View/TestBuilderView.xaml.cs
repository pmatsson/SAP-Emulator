using Builder.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Builder.View
{
    /// <summary>
    /// Interaction logic for TriggerView.xaml
    /// </summary>
    public partial class TestBuilderView : UserControl
    {
        public TestBuilderView()
        {
            InitializeComponent();
        }

        public void AddRow()
        {
            if (DataContext is RuleViewModel)
            {
                var triggContext = DataContext as RuleViewModel;
                triggContext.CreateRule();
            }
        }

        public void DeleteSelectedRow()
        {
            if (DataContext is RuleViewModel && RuleDG.SelectedItem != null)
            {
                var triggContext = DataContext as RuleViewModel;

                triggContext.RemoveRule(RuleDG.SelectedItem as Rule);
            }
        }
    }
}
