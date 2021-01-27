using PlGui.ViewModels.Bus;
using Prism.Mvvm;
using System.Windows.Controls;

namespace PlGui.Views.Stops
{
    /// <summary>
    /// Interaction logic for AddBusStop.xaml
    /// </summary>
    public partial class AddBusStop : UserControl
    {
        public AddBusStop()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(AddBusStop).ToString(), typeof(AddBusViewModel));
        }
    }
}
