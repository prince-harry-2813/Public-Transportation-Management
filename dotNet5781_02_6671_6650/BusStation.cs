using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_6671_6650
{
    abstract class BusStation
    {
        internal int stationCode { get; private set; } = 0;
        string adress { get; set; } 
        double latitude { get; set; }
        double longitude { get; set; }

        public BusStation(int _code, double _lat, double _long, string _adress="") {
            stationCode = _code;
            latitude = _lat;
            longitude = _long;
            adress = _adress;
        }

        public override string ToString() => $"Bus Station Code: {stationCode}, {latitude}°N {longitude}°E\n";

    }
}
