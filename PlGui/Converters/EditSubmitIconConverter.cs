using FontAwesome5;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PlGui.Converters
{
    public class EditSubmitIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                bool edit = (bool)value;
                if (edit)
                {
                    return EFontAwesomeIcon.Solid_PencilAlt;
                }
                else
                {
                    return EFontAwesomeIcon.Solid_Check;
                }
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
