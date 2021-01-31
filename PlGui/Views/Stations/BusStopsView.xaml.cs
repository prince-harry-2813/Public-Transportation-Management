using System.Windows.Controls;
using System.Windows.Input;
using PlGui.ViewModels.Stations;
using Prism.Mvvm;

namespace PlGui.Views.Stations
{
    /// <summary>
    /// Interaction logic for BusStopsView.xaml
    /// </summary>
    public partial class BusStopsView : UserControl
    {
        private BusStopsViewViewModel viewModel;
        public BusStopsView()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(BusStopsView).ToString(), typeof(BusStopsViewViewModel));
            viewModel = (BusStopsViewViewModel) this.DataContext;
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel = (BusStopsViewViewModel)this.DataContext;
            viewModel.UpdateBusStopButtonCommand.Execute(null);
        }
    }
}
