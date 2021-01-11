﻿using System;
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
using PlGui.ViewModels.Lines;
using Prism.Mvvm;

namespace PlGui.Views.Lines
{
    /// <summary>
    /// Interaction logic for LinesView.xaml
    /// </summary>
    public partial class LinesView : UserControl
    {
        public LinesView()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(LinesView).ToString(), typeof(LinesViewViewModel));
        }
    }
}