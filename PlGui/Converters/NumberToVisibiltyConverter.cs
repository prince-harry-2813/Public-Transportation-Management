using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PlGui.Converters
{
    public class NumberToVisibiltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && value != null)
            {

                var visibilty = Visibility.Visible;

                if ((int)value == 0)
                {
                    visibilty = Visibility.Collapsed;
                }

                return visibilty;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
