using Builder.MQ;
using Builder.Processor;
using Builder.ViewModel;
using MahApps.Metro.Controls;
using System;
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
            if (_RuleViewModel.OpenDocument == "" || _RuleViewModel.OpenDocument == null)
                SaveAsButton_Click(sender, e);
            else
                _RuleViewModel.SerializeRules(_RuleViewModel.OpenDocument);
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "EmulatorRules"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

            var result = dlg.ShowDialog();

            if (result == true)
            {
                _RuleViewModel.SerializeRules(dlg.FileName);
                _RuleViewModel.OpenDocument = dlg.FileName;
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            var result = dlg.ShowDialog();
            if (result == true && dlg.CheckPathExists)
            {
                _RuleViewModel.DeSerializeRules(dlg.FileName);
                _RuleViewModel.OpenDocument = dlg.FileName;
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            if(_RuleViewModel.Rules.Count != 0)
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
                        _RuleViewModel.Rules.Clear();
                        break;
                    case MessageBoxResult.No:
                        _RuleViewModel.Rules.Clear();
                        break;
                    case MessageBoxResult.Cancel:
                        
                        break;
                }
            }
        }




        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _RuleViewModel.CancelEmulation();
        //}
    }
}
