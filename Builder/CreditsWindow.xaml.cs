using System;
using System.Windows;

namespace MQChatter
{
    /// <summary>
    /// Interaction logic for CreditsWindow.xaml
    /// </summary>
    public partial class CreditsWindow : Window
    {
        public CreditsWindow()
        {
            InitializeComponent();
            TextBlockCredits.Text = Properties.Resources.ControlzEx_license + Environment.NewLine + Environment.NewLine;
            TextBlockCredits.Text += Properties.Resources.NLog_license + Environment.NewLine + Environment.NewLine;
            TextBlockCredits.Text += Properties.Resources.MvvmLight_license + Environment.NewLine + Environment.NewLine;
            TextBlockCredits.Text += Properties.Resources.MaterialDesignThemes_license + Environment.NewLine + Environment.NewLine;
            TextBlockCredits.Text += Properties.Resources.MaterialDesignColors_license + Environment.NewLine + Environment.NewLine;
            TextBlockCredits.Text += Properties.Resources.Font_Awesome_WPF_license + Environment.NewLine + Environment.NewLine;
            TextBlockCredits.Text += Properties.Resources.FontAwesome_License + Environment.NewLine + Environment.NewLine;
            TextBlockCredits.Text += Properties.Resources.gong_wpf_dragdrop_license + Environment.NewLine + Environment.NewLine;
        }
    }
}