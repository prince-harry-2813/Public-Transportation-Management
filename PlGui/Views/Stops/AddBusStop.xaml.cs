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
using PlGui.ViewModels.Bus;
using PlGui.ViewModels.Lines;
using PlGui.Views.Lines;
using Prism.Mvvm;

namespace PlGui.Views.Stops
{
    /// <summary>
    /// Interaction logic for AddBusStop.xaml
    /// </summary>
    public partial class AddBusStop : UserControl
    {
        public AddBusStop()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(AddBusStop).ToString(), typeof(AddBusViewModel));
        }
    }
}
