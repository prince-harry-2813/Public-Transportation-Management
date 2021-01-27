using PlGui.ViewModels.Stops;
using Prism.Mvvm;
using System.Windows.Controls;

namespace PlGui.Views.Stops
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
    }
}
