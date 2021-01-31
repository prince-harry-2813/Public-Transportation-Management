using PlGui.ViewModels.Lines;
using Prism.Mvvm;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlGui.Views.Lines
{
    /// <summary>
    /// Interaction logic for LinesView.xaml
    /// </summary>
    public partial class LinesView : UserControl
    {
        private LinesViewViewModel viewModel;

        public LinesView()
        {
            InitializeComponent();
            ViewModelLocationProvider.Register(typeof(LinesView).ToString(), typeof(LinesViewViewModel));
            viewModel = (LinesViewViewModel) this.DataContext;
        }


        //private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    viewModel = (LinesViewViewModel)this.DataContext;
        //    viewModel.ChooseLineButtonCommand.Execute("");
        //}
        private void LbBuses_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            viewModel = (LinesViewViewModel)this.DataContext;
            viewModel.UpdateLineButtonCommand.Execute(null);
        }
    }
}
