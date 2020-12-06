using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace dotNet5781_03B_6671_6650.Converters
{
    public enum StatusEnum 
    {
        Ok = 1,
        NOk = 2,
        Warning
    }

    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
                    case StatusEnum.NOk:
                        color = new SolidColorBrush(Colors.Red);
                        break;
                    case StatusEnum.Warning:
                        color = new SolidColorBrush(Colors.Yellow);
                        break;  
                    default:
                        color = new SolidColorBrush(Colors.White);
                        break;
                }

                return color;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
