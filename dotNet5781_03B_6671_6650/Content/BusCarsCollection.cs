using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace dotNet5781_03B_6671_6650.Content
{
    /// <summary>
    /// singleton class to hold the bus info for the project
    /// </summary>
    public class BusCarsCollection
    {
        private BusCarsCollection()
        {
            ObservableCollection<Bus> BusesCollection = new ObservableCollection<Bus>();
        }
     
        static BusCarsCollection()
        {
            BusesCollection = new ObservableCollection<Bus>();
        }
        
        private static readonly object obj = new object();
        
        private static BusCarsCollection instance = null;
        
        public static ObservableCollection<Bus> BusesCollection { get; set; }
        
        public static BusCarsCollection Instance
        {
            get
            {
                lock (obj)
                {
                    if (instance == null)
                    {
                        instance = new BusCarsCollection();
                    }
                    return instance;
                }
            }
        }
    }
}
