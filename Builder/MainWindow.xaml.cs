using Builder.MQ;
using Builder.Processor;
using Builder.ViewModel;
using MahApps.Metro.Controls;
using System.Windows;

namespace Builder
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private RuleViewModel _RuleViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            _RuleViewModel = new RuleViewModel();

            this.DataContext = _RuleViewModel;
            TestBuilderView.DataContext = _RuleViewModel;
            EmulatorView.DataContext = _RuleViewModel;
            EmulatorView.CancelButton.Click += CancelButton_Click;
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _RuleViewModel.CancelEmulation();
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            TestBuilderView.AddRow();
        }


        private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
        {
            TestBuilderView.DeleteSelectedRow();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _RuleViewModel.StartEmulation();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _RuleViewModel.SerializeRules();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            _RuleViewModel.DeSerializeRules();
        }

        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _RuleViewModel.CancelEmulation();
        //}
    }
}
