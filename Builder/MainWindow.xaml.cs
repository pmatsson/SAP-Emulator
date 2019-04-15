using MahApps.Metro.Controls;
using MQChatter.ViewModel;
using System.Windows;

namespace MQChatter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MyPresenter.DataContext = DataContext;
            //this.DataContext = _ruleViewModel = new RuleViewModel();
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RuleViewModel).CreateRule();
        }

        private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RuleViewModel).RemoveSelectedRule();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RuleViewModel).StartEmulation();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RuleViewModel).CancelEmulation();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            RuleViewModel rvm = (DataContext as RuleViewModel);
            if (rvm.OpenDocument == "" || rvm.OpenDocument == null)
            {
                SaveAsButton_Click(sender, e);
            }
            else
            {
                rvm.SerializeRules(rvm.OpenDocument);
            }
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            RuleViewModel rvm = (DataContext as RuleViewModel);
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "EmulatorRules", // Default file name
                DefaultExt = ".xml", // Default file extension
                Filter = "XML documents (.xml)|*.xml" // Filter files by extension
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                rvm.SerializeRules(dlg.FileName);
                rvm.OpenDocument = dlg.FileName;
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            RuleViewModel rvm = (DataContext as RuleViewModel);
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if (result == true && dlg.CheckPathExists)
            {
                rvm.DeSerializeRules(dlg.FileName);
                rvm.OpenDocument = dlg.FileName;
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            RuleViewModel rvm = (DataContext as RuleViewModel);
            if (rvm.RuleGroups.Count != 0)
            {
                string messageBoxText = "Do you want to save changes?";
                string caption = "Emulator";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveButton_Click(sender, e);
                        rvm.RuleGroups.Clear();
                        break;

                    case MessageBoxResult.No:
                        rvm.RuleGroups.Clear();
                        break;

                    case MessageBoxResult.Cancel:

                        break;
                }
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var w = new HelpWindow();
            w.Show();
            w.Activate();
        }

        private void XPathTest_Click(object sender, RoutedEventArgs e)
        {
            var w = new XPathTestWindow();
            w.Show();
            w.Activate();
        }
    }
}