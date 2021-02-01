using PlGui.ViewModels.Lines;
using Prism.Mvvm;
using System.Windows.Controls;

namespace PlGui.Views.Lines
{
    /// <summary>
    /// Interaction logic for LineDetails.xaml
    /// </summary>
    public partial class LineDetails : UserControl
    {
        public LineDetails()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(LineDetails).ToString(), typeof(LineDetailsViewModel));
        }
    }
}
