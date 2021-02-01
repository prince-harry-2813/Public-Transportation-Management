using BL.BLApi;
using PlGui.StaticClasses;
using PlGui.Views;
using PlGui.Views.Bus;
using PlGui.Views.Lines;
using Prism.Ioc;
using System.Windows;
using PlGui.Views.Stations;

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
            containerRegistry.RegisterForNavigation<StartPage>(StringNames.StartPage);
            containerRegistry.RegisterForNavigation<ManagerLogin>(StringNames.ManagerLogin);
            containerRegistry.RegisterForNavigation<BusStopsView>(StringNames.BusStopsView);
            containerRegistry.RegisterForNavigation<AddBus>(StringNames.AddBus);
            containerRegistry.RegisterForNavigation<BusDetails>(StringNames.BusDetails);
            containerRegistry.RegisterForNavigation<BusesView>(StringNames.BusesView);
            containerRegistry.RegisterForNavigation<AddLine>(StringNames.AddLine);
            containerRegistry.RegisterForNavigation<LinesView>(StringNames.LinesView);
            containerRegistry.RegisterForNavigation<LineDetails>(StringNames.LineDetails);
            containerRegistry.RegisterForNavigation<AddBusStop>(StringNames.AddBusStop);
            containerRegistry.RegisterForNavigation<BusStopDetails>(StringNames.BusStopDetails);
            containerRegistry.RegisterForNavigation<BusStopsView>(StringNames.BusStopsView);
            containerRegistry.RegisterForNavigation<UserSimulation>(StringNames.UserSimulation);

            containerRegistry.RegisterSingleton<IBL>(BLFactory.GetIBL);
        }
    }
}
