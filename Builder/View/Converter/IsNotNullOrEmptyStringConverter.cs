using System;
using System.Globalization;
using System.Windows.Data;

namespace MQChatter.View.Converter
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class IsNotNullOrEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value == null || value.ToString() == "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}