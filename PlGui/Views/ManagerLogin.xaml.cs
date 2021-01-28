using PlGui.ViewModels;
using Prism.Mvvm;
using System.Windows.Controls;

namespace PlGui.Views
{
    /// <summary>
    /// Interaction logic for ManagerLogin.xaml
    /// </summary>
    public partial class ManagerLogin : UserControl
    {
        public ManagerLogin()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(ManagerLogin).ToString(), typeof(MainWindowViewModel));
        }
    }
}
