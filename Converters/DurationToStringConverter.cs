using System;
using System.Globalization;
using System.Windows.Data;

namespace MusicPlus.Converters
{
    public class DurationToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int seconds)
            {
                return FormatDuration(seconds);
            }
            if (value is long longSeconds)
            {
                return FormatDuration((int)longSeconds);
            }
            return "00:00";
        }

        private string FormatDuration(int seconds)
        {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            int secs = seconds % 60;

            if (hours > 0)
            {
                return $"{hours:D2}:{minutes:D2}:{secs:D2}";
            }
            return $"{minutes:D2}:{secs:D2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
