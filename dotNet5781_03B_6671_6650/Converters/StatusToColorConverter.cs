using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace dotNet5781_03B_6671_6650.Converters
{
    /// <summary>
    /// current Status of the bus
    /// </summary>
    public  enum StatusEnum
    {
        Ok = 1,
        InRide,
        InRefuling,
        InMaintainceing
    }
    /// <summary>
    ///   converter from status of bus to current situation color 
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
                    case StatusEnum.InRide:
                        color = new SolidColorBrush(Colors.Red);
                        break;
                    case StatusEnum.InRefuling:
                        color = new SolidColorBrush(Colors.Yellow);
                        break;
                    case StatusEnum.InMaintainceing:
                        color = new SolidColorBrush(Colors.Chocolate);
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
