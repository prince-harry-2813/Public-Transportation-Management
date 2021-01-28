using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using PlGui.ViewModels;
using PlGui.ViewModels.Bus;

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
            viewModel = (UserSimulationViewModel) this.DataContext;
            LastArrivadBusVM = (BusDetailsViewModel)viewModel.BusDetailsDataContext;
        }

        private void DatePickerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.ComboBoxSelectionChanged();
        }
    }
}
