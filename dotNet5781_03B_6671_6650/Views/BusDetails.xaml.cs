using dotNet5781_03B_6671_6650.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dotNet5781_03B_6671_6650.Views
{
    /// <summary>
    /// Interaction logic for BusDetails.xaml
    /// </summary>
    public partial class BusDetails : Window
    {
        public Bus SelectedBus { get; set; }
        List<BusPropertyInfo> busPropertyInfos { get; set; } = new List<BusPropertyInfo>();

        public BusDetails(Bus bus)
        {

            SelectedBus = bus;
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Bus Status :"
                ,
                PropertyValue = SelectedBus.BusStaus.ToString(),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "First Registration :"
              ,
                PropertyValue = SelectedBus.FirstRegistration.ToString(),

            }); busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Fuel :"
               ,
                PropertyValue = SelectedBus.Fuel.ToString(),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Last Treatment :"
               ,
                PropertyValue = SelectedBus.LastTreatment.ToString(),

            });
            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Licens Nmuber :"
               ,
                PropertyValue = SelectedBus.LicensNmuber.ToString(),
            });

            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Maintenance :"
               ,
                PropertyValue = SelectedBus.Maintenance.ToString(),
            });

            busPropertyInfos.Add(new BusPropertyInfo
            {
                PropertyKey = "Total KM :"
               ,
                PropertyValue = SelectedBus.TotalKM.ToString(),
            });
            InitializeComponent();
            lbBusDetails.ItemsSource = busPropertyInfos;
            //PropertyInfo propertyInfo;
            //propertyInfo.
        }

        private void RefuleButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedBus.ReFuelBus();
            Task.Factory.StartNew(() => {
                SelectedBus.BusStaus = StatusEnum.NOk;
                Thread.Sleep(12000);
                SelectedBus.BusStaus = StatusEnum.Ok;
            });
        }

        private void TreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedBus.MaintaineBus();
            Task.Factory.StartNew(() => {
                SelectedBus.BusStaus = StatusEnum.NOk;
                Thread.Sleep(6 * 24 * 1000);
                SelectedBus.BusStaus = StatusEnum.Ok;
            });
        }
    }

    class BusPropertyInfo
    {
        public string PropertyKey { get; set; }
        public string PropertyValue { get; set; }
    }
}
