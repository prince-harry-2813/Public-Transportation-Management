using PlGui.ViewModels;
using PlGui.ViewModels.Bus;
using System.Windows.Controls;

namespace PlGui.Views
{
    /// <summary>
    /// Interaction logic for UserSimulation.xaml
    /// </summary>
    public partial class UserSimulation : UserControl
    {
        private BusDetailsViewModel LastArrivadBusVM;
        private UserSimulationViewModel viewModel;
        public UserSimulation()
        {
            InitializeComponent();
            viewModel = (UserSimulationViewModel)this.DataContext;
            LastArrivadBusVM = (BusDetailsViewModel)viewModel.BusDetailsDataContext;
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.ComboBoxSelectionChanged();
        }
    }
}
