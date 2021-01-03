using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PlGui.Converters
{
    /// <summary>
    /// current Status of the bus
    /// </summary>
    public  enum StatusEnum
    {
        Ok = 1,
        In_Ride,
        In_Refuling,
        In_Maintainceing,
        Not_Available
    }
    /// <summary>
    ///  available converter from status of bus to current situation color 
    /// </summary>
    public class StatusToColorConverter : IValueConverter
    {
        public object  Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatusEnum && value != null)
            {
                StatusEnum status = (StatusEnum)value;
                var color = new SolidColorBrush(Colors.Black);

                switch (status)
                {
                    case StatusEnum.Ok:
                        color = new SolidColorBrush(Colors.Green);
                        break;
                    case StatusEnum.In_Ride:
                        color = new SolidColorBrush(Colors.Blue);
                        break;
                    case StatusEnum.In_Refuling:
                        color = new SolidColorBrush(Colors.Gold);
                        break;
                    case StatusEnum.In_Maintainceing:
                        color = new SolidColorBrush(Colors.Chocolate);
                        break;
                    case StatusEnum.Not_Available:
                        color = new SolidColorBrush(Colors.Red);
                        break;
                    default:
                        color = new SolidColorBrush(Colors.White);
                        break;
                }

                return color;
            }
            return Binding.DoNothing;
        }

        public  object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
