using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_6671_6650
{
    class linesCollection : IEnumerable
    {
        List<BusLine> busCollection = new List<BusLine>();

        public List<BusLine> linesOnStation(int stopCode)
        {
            return null;
        }

        public List<BusLine> sorterLines()
        {
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        // Define the indexer to allow client code to use [] notation.
        public BusLine this[int i]
        {

            get
            {

                return this.busCollection.FirstOrDefault(BusLine => BusLine.LineKey == i);
            }
        }
    }
}
