using Builder.MQ;
using Builder.Processor;
using Builder.ViewModel;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Builder
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
            var rvm = (DataContext as RuleViewModel);
            if (rvm.OpenDocument == "" || rvm.OpenDocument == null)
                SaveAsButton_Click(sender, e);
            else
                rvm.SerializeRules(rvm.OpenDocument);
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            var rvm = (DataContext as RuleViewModel);
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "EmulatorRules"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

            var result = dlg.ShowDialog();

            if (result == true)
            {
                rvm.SerializeRules(dlg.FileName);
                rvm.OpenDocument = dlg.FileName;
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var rvm = (DataContext as RuleViewModel);
            var dlg = new Microsoft.Win32.OpenFileDialog();
            var result = dlg.ShowDialog();
            if (result == true && dlg.CheckPathExists)
            {
                rvm.DeSerializeRules(dlg.FileName);
                rvm.OpenDocument = dlg.FileName;
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var rvm = (DataContext as RuleViewModel);
            if (rvm.Rules.Count != 0)
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
                        rvm.Rules.Clear();
                        break;
                    case MessageBoxResult.No:
                        rvm.Rules.Clear();
                        break;
                    case MessageBoxResult.Cancel:
                        
                        break;
                }
            }
        }
    }
}
