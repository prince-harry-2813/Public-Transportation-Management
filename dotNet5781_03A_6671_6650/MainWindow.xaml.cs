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

        public LinesCollection lines = new LinesCollection();

        private BusLine currentDisplayBusLine;

        public volatile Random Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineKey"></param>
        private void ShowBusline(int lineKey)
        {
            currentDisplayBusLine = lines[lineKey];
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStation.DataContext = currentDisplayBusLine.LineStations;
          

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusline((cbBusLines.SelectedValue as BusLine).LineKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="stopCode"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool isNewExist(int line, int stopCode, int? pos = null)
        {
            foreach (BusLine item in lines)
            {
                if (item.IsExist(stopCode))
                {
                    lines[line].AddStop(new BusStop( stopCode,item.LineStations.FirstOrDefault(stop => stop.StationCode == (stopCode)).Latitude, item.LineStations.FirstOrDefault(stop => stop.StationCode == (stopCode)).Longitude), pos);
                    return true;
                }
            }
            return false;
        }

        public MainWindow()
        {
            try
            {


                InitializeComponent();
                for (int j = 0; j <= 20; j++)
                {
                    lines.Add(new BusLine(Random.Next(j, 999)));
                    for (int i = 0; i <= 20; i++)
                    {
                        BusStop stop = new BusStop((i + j + 300), Random.NextDouble() * 2.3 + 31, Random.NextDouble() * 1.2 + 34.3);
                        if (isNewExist(lines.Last().LineKey, stop.StationCode, lines.Last().LineStations.Count))
                            continue;
                        lines.Last().AddStop(stop, lines.Last().LineStations.Count);
                    }
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.GetType().ToString() + ' ' + e.Message.ToString());

            }
            cbBusLines.ItemsSource = lines;
            cbBusLines.DisplayMemberPath = "LineKey";
            cbBusLines.SelectedIndex = 0;
            ShowBusline(lines.FirstOrDefault().LineKey);
        }


    }
}
