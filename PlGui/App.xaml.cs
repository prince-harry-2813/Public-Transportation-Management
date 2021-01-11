using PlGui.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using PlGui.StaticClasses;
using PlGui.Views.Bus;
using PlGui.Views.Lines;
using PlGui.Views.Stops;
using Prism.Unity;

namespace PlGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(object), typeof(StartPage), StringNames.StartPage);
            containerRegistry.Register(typeof(object), typeof(ManagerLogin), StringNames.ManagerLogin);
            containerRegistry.Register(typeof(object), typeof(BusStopsView), StringNames.BusStopsView);
            containerRegistry.Register(typeof(object), typeof(AddBus), StringNames.AddBus);
            containerRegistry.Register(typeof(object), typeof(BusDetails), StringNames.BusDetails);
            containerRegistry.Register(typeof(object), typeof(BusesView), StringNames.BusesView);
            containerRegistry.Register(typeof(object), typeof(AddLine), StringNames.AddLine);
            containerRegistry.Register(typeof(object), typeof(LinesView), StringNames.LinesView);
            containerRegistry.Register(typeof(object), typeof(LineDetails), StringNames.LineDetails);
            containerRegistry.Register(typeof(object), typeof(AddBusStop), StringNames.AddBusStop);
            containerRegistry.Register(typeof(object), typeof(BusStopDetails), StringNames.BusStopDetails);
            containerRegistry.Register(typeof(object), typeof(BusStopsView), StringNames.BusStopsView);
        }
    }
}
