using PlGui.ViewModels.Bus;
using PlGui.ViewModels.Stops;
using Prism.Mvvm;
using System.Diagnostics;
using System.Windows.Controls;

namespace PlGui.Views.Stops
{
    /// <summary>
    /// Interaction logic for AddBusStop.xaml
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
