using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicPlus.Converters
{
    public class PremiumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.ToString()?.Equals("Premium", StringComparison.OrdinalIgnoreCase) == true)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff943d"));
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
