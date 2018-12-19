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
        private RuleViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new RuleViewModel();

            this.DataContext = ViewModel;
            TestBuilderView.DataContext = ViewModel;
            
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            TestBuilderView.AddRow();
        }

        private void GenerateXML_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SerializeRules();
        }

        private void LoadXML_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeSerializeRules();
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            EditFlyout.IsOpen = true;
        }

        private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
        {
            TestBuilderView.DeleteSelectedRow();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartTest();
        }
    }
}
