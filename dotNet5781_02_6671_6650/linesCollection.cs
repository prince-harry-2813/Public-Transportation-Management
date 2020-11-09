using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_6671_6650
{
    class LinesCollection : IEnumerable<BusLine>
    {
        List<BusLine> busCollection = new List<BusLine>();

        public List<BusLine> linesOnStation(int stopCode)
        {
            return null;
        }
        public void Add(BusLine newer)
        {
            if (this.busCollection.Exists(BusLine => BusLine.LineKey == newer.LineKey && BusLine.FirstStation.StationCode == newer.FirstStation.StationCode))
            {
                throw new ArgumentException("this line already exist in the same direction");
            }
            else
                busCollection.Add(newer);
        }
        public List<BusLine> sorterLines()
        {
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<BusLine> IEnumerable<BusLine>.GetEnumerator()
        {
            return ((IEnumerable<BusLine>)busCollection).GetEnumerator();
        }

        // Define the indexer to allow client code to use [] notation.
        public List <BusLine> this[int i]
        {
            get
            {
                return this.busCollection.FindAll(BusLine => BusLine.LineKey == i);
            }
        }
    }
}
