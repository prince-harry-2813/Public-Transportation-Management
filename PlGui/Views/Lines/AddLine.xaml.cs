using Prism.Mvvm;
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
using PlGui.ViewModels.Lines;

namespace PlGui.Views.Lines
{
    /// <summary>
    /// Interaction logic for AddLine.xaml
    /// </summary>
    public partial class AddLine : UserControl
    {
        public AddLine()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(AddLine).ToString(), typeof(AddLineViewModel));
        }
    }
}
