using PlGui.ViewModels.Lines;
using Prism.Mvvm;
using System.Windows.Controls;

namespace PlGui.Views.Lines
{
    /// <summary>
    /// Interaction logic for LinesView.xaml
    /// </summary>
    public partial class LinesView : UserControl
    {
        public LinesView()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(LinesView).ToString(), typeof(LinesViewViewModel));
        }

     
    }
}
