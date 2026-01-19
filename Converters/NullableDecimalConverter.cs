using System;
using System.Globalization;
using System.Windows.Data;

namespace MusicPlus.Converters
{
    public class NullableDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("F1", culture);
            }
            return "";
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && decimal.TryParse(str, NumberStyles.Any, culture, out decimal result))
            {
                return result;
            }
            return null;
        }
    }
}
