using PlGui.ViewModels.Lines;
using Prism.Mvvm;
using System.Windows.Controls;

namespace PlGui.Views.Lines
{
    /// <summary>
    /// Interaction logic for AddLine.xaml
    /// </summary>
    public partial class AddLine : UserControl
    {
        public AddLine()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(AddLine).ToString(), typeof(AddLineViewModel));
        }
    }
}
