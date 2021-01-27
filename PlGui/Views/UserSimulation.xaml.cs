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
            //LastArrivadBusVM = (BusDetailsViewModel)BusDetailsUC.DataContext;
            //if (LastArrivadBusVM == null)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        Thread.Sleep(3000);
            //    });
            //    LastArrivadBusVM = (BusDetailsViewModel)BusDetailsUC.DataContext;
            //    LastArrivadBusVM.InternalReadOnly = true;
            //    LastArrivadBusVM.ButtonsVisibility = false;
            //    LastArrivadBusVM.MainLabelContent = "Last Arrived Bus";
            //}
            //else
            //{

            //    LastArrivadBusVM.InternalReadOnly = true;
            //    LastArrivadBusVM.ButtonsVisibility = false;
            //    LastArrivadBusVM.MainLabelContent = "Last Arrived Bus";

            //}

        }

        private void DatePickerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
