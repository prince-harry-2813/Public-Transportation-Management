using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_6671_6650
{

    class BusStop : BusStation
    {
        double distance{ get; set; }
        DateTime arrivingTime { get; set; }

        public BusStop(int _code, double _lat, double _long, string _adress = "") : base(_code, _lat, _long, _adress)
        {
        }
    }
}
