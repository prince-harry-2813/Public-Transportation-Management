using PlGui.ViewModels;
using Prism.Mvvm;
using System.Windows.Controls;

namespace PlGui.Views
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        public StartPage()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(StartPage).ToString(), typeof(MainWindowViewModel));
        }
    }
}
