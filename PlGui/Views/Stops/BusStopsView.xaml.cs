using System.Windows.Controls;
using PlGui.ViewModels.Lines;
using PlGui.ViewModels.Stops;
using PlGui.Views.Lines;
using Prism.Mvvm;

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
