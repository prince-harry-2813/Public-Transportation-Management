using System.Windows.Controls;
using PlGui.ViewModels.Stations;
using Prism.Mvvm;

namespace PlGui.Views.Stations
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
