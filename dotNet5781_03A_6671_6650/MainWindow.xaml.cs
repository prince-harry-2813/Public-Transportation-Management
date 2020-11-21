using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using dotNet5781_02_6671_6650;

namespace dotNet5781_03A_6671_6650
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public BusLine currentDisplayBusLine;
        public LinesCollection BusLines;

        public static Random Random = new Random();

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                BusLines = new LinesCollection() { };

                for (int i = 0; i < 20; i++)
                {
                    BusLines.Add(new BusLine(Random.Next(1, 999), Random.Next(1, 100), Random.Next(100, 200)));
                }
                foreach (BusLine item in BusLines)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        BusStop stop = new BusStop(i + 300, Random.NextDouble() * 2.3 + 31, Random.NextDouble() * 1.2 + 34.3);
                        if(isNewExist(item.LineKey, stop.StationCode, item.LineStations.Count - 2))
                         continue;
                        item.AddStop(stop, item.LineStations.Count - 2);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }

            

            cbBusLines.ItemsSource = BusLines;
            cbBusLines.DisplayMemberPath = "LineKey";
            cbBusLines.SelectedIndex = 0;
            ShowBusLine(BusLines.FirstOrDefault().LineKey);
        }

        private bool isNewExist(int lineKey, int stationCode, int v)
        {
            foreach (BusLine item in BusLines)
            {
                if (item.IsExist(stationCode))
                {
                    BusLines[lineKey].AddStop(item.LineStations.FirstOrDefault(stop => stop.StationCode == (stationCode)), v);
                    return true;
                }
            }
            return false;
        }

        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = BusLines[index];
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.LineStations;
        }

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).LineKey);
        }
    }
}
