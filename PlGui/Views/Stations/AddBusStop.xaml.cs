using System.Diagnostics;
using System.Windows.Controls;
using PlGui.ViewModels.Stations;

namespace PlGui.Views.Stations
{
    /// <summary>
    /// Interaction logic for AddStation.xaml
    /// </summary>
    public partial class AddBusStop : UserControl
    {
        private AddBusStopViewModel viewModel;
        public AddBusStop()
        {
            InitializeComponent();
            viewModel = (AddBusStopViewModel)this.DataContext;
            Debug.Print(viewModel.ToString());
        }

        private void Button_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            viewModel.AddBusStopButtonCommand.Execute(null);
        }
    }
}
