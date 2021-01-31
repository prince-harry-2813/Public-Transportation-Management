using System.Windows.Controls;
using PlGui.ViewModels.Stations;
using Prism.Mvvm;

namespace PlGui.Views.Stations
{
    /// <summary>
    /// Interaction logic for BusStopsView.xaml
    /// </summary>
    public partial class BusStopsView : UserControl
    {
        public BusStopsView()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(BusStopsView).ToString(), typeof(BusStopsViewViewModel));

        }

        private void LbBuses_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
