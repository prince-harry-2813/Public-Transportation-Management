using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace dotNet5781_02_6671_6650
{
    /// <summary>
    /// class that represent a system collection of lines, the class hold an private list of lines 
    /// </summary>
    class LinesCollection : IEnumerable<BusLine>
    {
        /// <summary>
        /// Implement of new exception type for station that no have a use
        /// </summary>
        [Serializable]
        public class StationNotUsedException : Exception
        {
            public StationNotUsedException() { }
            public StationNotUsedException(string message) : base(message) { }
            public StationNotUsedException(string message, Exception inner) : base(message, inner) { }
            protected StationNotUsedException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public override string ToString()
            {
                return ("There is no line's that using this station" + Message);
            }
        }

      private  List<BusLine> busCollection = new List<BusLine>();
        /// <summary>
        /// implement of IEnumerable on line collection class, return line that store in the internal list of line
        /// </summary>
        /// <returns></returns>
        IEnumerator<BusLine> IEnumerable<BusLine>.GetEnumerator()
        {
            return this.busCollection.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)busCollection).GetEnumerator();
        }



        /// <summary>
        /// return list of lines that have a stop in the station
        /// </summary>
        /// <param name="stopCode"></param>
        /// <returns></returns>
        public List<BusLine> LinesOnStation(int stopCode)
        {
            List<BusLine> linesOfStop = new List<BusLine>();
            foreach (BusLine item in this)
            {
                if (item.IsExist(stopCode))
                {
                    linesOfStop.Add(item);
                }
            }
            return linesOfStop.Count != 0 ? linesOfStop : throw new StationNotUsedException("Line over this Station: 0");
        }
        /// <summary>
        /// method to register new line in the system collection
        /// </summary>
        /// <param name="newer"></param>
        public void Add(BusLine newer)
        {
            if (this.busCollection.Exists(BusLine => BusLine.LineKey == newer.LineKey && BusLine.LastStaion.StationCode == newer.LastStaion.StationCode))
            {
                throw new ArgumentException("this line already exist in the same direction");
            }
            else if (busCollection.Exists(BusLine => BusLine.LineKey == newer.LineKey))
            {
                newer.FirstStation = this[newer.LineKey].LastStaion;
                newer.LastStaion = this[newer.LineKey].FirstStation;
            }

            busCollection.Add(newer);
        }
        /// <summary>
        /// sort lines by time of riding
        /// </summary>
        /// <returns></returns>
        public LinesCollection SorterLines()
        {

            this.busCollection.Sort();
            this.GetEnumerator().Reset();
            return this;
        }
        /// <summary>
        /// remove line from system collection
        /// </summary>
        /// <param name="delete"></param>
        public void Remove(BusLine delete)
        {
            this.busCollection.Remove(delete);
            Console.WriteLine("Remove complete");
        }




        /// <summary>
        ///Define the indexer to allow client code to use '[]' notation.
        /// </summary>
        /// <param name="i">index number</param>
        /// <returns>line number -> i</returns>
        public BusLine this[int i]
        {
            get
            {
                List<BusLine> line = new List<BusLine>();

                foreach (BusLine item in this)
                {
                    if (i == item.LineKey)
                    {
                        line.Add(item);
                    }
                };
                if (line.Count == 1)
                {
                    return line.First<BusLine>();
                }
                else if (line.Count == 0)
                {
                    throw new ArgumentOutOfRangeException($"there is no line number {i}");
                }
                else
                {
                    Console.WriteLine("There is tow direction for this line, To complete your operation please choose one by press 1 or 2");
                    return line.ElementAt(int.Parse(Console.ReadLine()) - 1);
                }
            }
        }
    }
}
