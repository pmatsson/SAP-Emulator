using MQChatter.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MQChatter
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GridWindowTitleMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void SubmenuOpenClose(object sender, MouseButtonEventArgs e)
        {
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, 500);
            int listViewItemHeight = int.Parse(Resources["ListViewItemHeight"].ToString());
            FrameworkElement fwe = sender as FrameworkElement;

            if (fwe != null && int.TryParse(fwe.Tag?.ToString(), out int expandedHeight))
            {
                if (fwe.Height == listViewItemHeight)
                {
                    AnimateHeight(sender, expandedHeight, duration);
                }
                else if (fwe.Height == expandedHeight)
                {
                    AnimateHeight(sender, listViewItemHeight, duration);
                }
            }
        }

        private void AnimateHeight(object obj, int height, TimeSpan duration)
        {
            if (obj is FrameworkElement)
            {
                FrameworkElement fwe = obj as FrameworkElement;
                DoubleAnimation animation = new DoubleAnimation(height, duration);
                fwe.BeginAnimation(FrameworkElement.HeightProperty, animation);
            }
        }

        private void ListViewItemExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
            e.Handled = true;
        }

        private void ListViewItemNew_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RuleViewModel rvm = (DataContext as RuleViewModel);
            if (rvm.RuleGroups.Count != 0)
            {
                string messageBoxText = "Do you want to save changes?";
                string caption = "MQ Chatter";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ListViewItemSaveAs_MouseLeftButtonUp(sender, e);
                        rvm.RuleGroups.Clear();
                        break;

                    case MessageBoxResult.No:
                        rvm.RuleGroups.Clear();
                        break;

                    case MessageBoxResult.Cancel:

                        break;
                }
            }
            e.Handled = true;
        }

        private void ListViewItemOpen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RuleViewModel rvm = (DataContext as RuleViewModel);
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if (result == true && dlg.CheckPathExists)
            {
                rvm.DeSerializeRules(dlg.FileName);
                rvm.OpenDocument = dlg.FileName;
            }
            e.Handled = true;
        }

        private void ListViewItemSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListViewItemSaveAs_MouseLeftButtonUp(sender, e);
            e.Handled = true;
        }

        private void ListViewItemSaveAs_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
            e.Handled = true;
        }

        private void ListViewItemAddRule_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (DataContext as RuleViewModel).CreateRule();
            e.Handled = true;
        }

        private void ListViewItemDeleteRule_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (DataContext as RuleViewModel).RemoveSelectedRule();
            e.Handled = true;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RuleViewModel).StartEmulation();
            e.Handled = true;
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RuleViewModel).CancelEmulation();
            e.Handled = true;
        }

        private void ListViewItemXpathTool_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            XPathTestWindow w = new XPathTestWindow();
            w.Show();
            w.Activate();
            e.Handled = true;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void ButtonStandardize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonCredits_Click(object sender, RoutedEventArgs e)
        {
            CreditsWindow w = new CreditsWindow();
            w.Show();
            w.Activate();
            e.Handled = true;
        }
    }
}