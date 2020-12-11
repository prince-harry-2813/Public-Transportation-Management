using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace dotNet5781_03B_6671_6650.Content
{
    /// <summary>
    /// singleton class to hold the bus info for the project
    /// </summary>
    public sealed class BusCarsCollection : DependencyObject , INotifyPropertyChanged
    {
        //private BusCarsCollection()
        //{
        //    ObservableCollection<Bus> BusesCollection = new ObservableCollection<Bus>();
        //}
     
        BusCarsCollection()
        {
            BusesCollection = new ObservableCollection<Bus>();
        }

        public ObservableCollection<Bus> BusesCollection
        {
            get { return (ObservableCollection<Bus>)GetValue(BusesCollectionProperty); }
            set { SetValue(BusesCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BusesCollection.  This enables animation, styling, binding, etc...
        public static readonly System.Windows.DependencyProperty BusesCollectionProperty =
            DependencyProperty.Register("BusesCollection", typeof(ObservableCollection<Bus>), typeof(BusCarsCollection), new PropertyMetadata(null));



        private static readonly object obj = new object();
        
        private static BusCarsCollection instance = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        //public static ObservableCollection<Bus> BusesCollection { get; set; }

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
