﻿using Builder.Model.Trigger;
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
                triggContext.AddRow();
            }
        }
    }
}
