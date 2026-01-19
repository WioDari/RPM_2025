using System;
using System.Globalization;
using System.Windows.Data;

namespace MusicPlus.Converters
{
    public class BlockButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBlocked)
            {
                return isBlocked ? "Разблокировать" : "Заблокировать";
            }
            return "Заблокировать";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
