using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace PlGui.Converters
{
    public class ParameterToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = (string)value;
            StringBuilder title = new StringBuilder();

            int i = 0;
            foreach (char variable in param)
            {
                if (char.IsUpper(variable))
                {
                    if (i > 0)
                    {
                        title.Append(" ");
                    }

                    i++;
                }
                title.Append(variable);
            }

            return title.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
