using Builder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Builder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContainerViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new ContainerViewModel();

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
    }
}
