using System;
using System.Windows;
using System.Windows.Controls;
using PlGui.ViewModels.Bus;

namespace PlGui.Views.Bus
{
    /// <summary>
    /// Interaction logic for BusDetails.xaml
    /// </summary>
    public partial class BusDetails : UserControl
    {
        private BusDetailsViewModel viewModel;
        public BusDetails()
        {
            InitializeComponent();
            viewModel = (BusDetailsViewModel) this.DataContext;
            lbBusDetails.DataContext = viewModel.Bl.GetBus(viewModel.LicenseNumber);
        }

        private void RefuleButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TreatmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
