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
using PlGui.ViewModels.Stops;
using PlGui.Views.Lines;
using Prism.Mvvm;

namespace PlGui.Views.Stops
{
    /// <summary>
    /// Interaction logic for BusStopDetails.xaml
    /// </summary>
    public partial class BusStopDetails : UserControl
    {
        public BusStopDetails()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(BusStopDetails).ToString(), typeof(BusStopDetailsViewModel));

        }
    }
}
