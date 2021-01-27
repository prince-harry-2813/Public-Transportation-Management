using PlGui.ViewModels.Stops;
using Prism.Mvvm;
using System.Windows.Controls;

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
