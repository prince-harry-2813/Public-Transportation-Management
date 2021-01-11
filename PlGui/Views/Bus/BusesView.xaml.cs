using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PlGui.ViewModels;
using PlGui.ViewModels.Bus;
using Prism.Mvvm;

namespace PlGui.Views.Bus
{
    /// <summary>
    /// Interaction logic for BusesView.xaml
    /// </summary>
    public partial class BusesView : UserControl
    {
        private BusesViewViewModel viewModel;
        public BusesView()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(BusesView).ToString(), typeof(BusesViewViewModel));
            viewModel = (BusesViewViewModel) this.DataContext;
        }


        private void LicensNumberLabel_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewModel == null)
            {
            viewModel = (BusesViewViewModel) this.DataContext;

            }
            viewModel.OnMouseDoubleClick.Execute(null);
        }

        private void FrameLB_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewModel == null)
            {
                viewModel = (BusesViewViewModel)this.DataContext;

            }
            viewModel.OnMouseDoubleClick.Execute(null);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (viewModel == null)
            {
                viewModel = (BusesViewViewModel)this.DataContext;
            }
            viewModel.OnMouseDoubleClick.Execute(null);
        }
    }
}
