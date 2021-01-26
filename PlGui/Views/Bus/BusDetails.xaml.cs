using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using PlGui.ViewModels.Bus;
using Prism.Mvvm;

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
           // ViewModelLocationProvider.Register(typeof(BusDetails).ToString(), typeof(BusDetailsViewModel));
            viewModel = (BusDetailsViewModel)this.DataContext;
        }

        private void RefuleButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TreatmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LbBusDetails_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel == null)
            {
                viewModel = (BusDetailsViewModel)this.DataContext;
            }
            viewModel.ListBoxSelectionChanged();
        }
    }
}