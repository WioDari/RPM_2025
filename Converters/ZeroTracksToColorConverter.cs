using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicPlus.Converters
{
    public class ZeroTracksToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int trackCount)
            {
                return trackCount == 0 
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CC3366"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1C1C1C"));
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1C1C1C"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
