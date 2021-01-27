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
            foreach (char VARIABLE in param)
            {
                if (char.IsUpper(VARIABLE))
                {
                    if (i > 0)
                    {
                        title.Append(" ");
                    }

                    i++;
                }
                title.Append(VARIABLE);
            }

            return title.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
