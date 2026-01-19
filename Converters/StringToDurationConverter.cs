using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace MusicPlus.Converters
{
    public class StringToDurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int seconds)
            {
                return FormatDuration(seconds);
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
            if (value is string str)
            {
                return ParseDuration(str);
            }
            return 0;
        }

        private int ParseDuration(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                return 0;
                
            try
            {
                duration = duration.Trim();
                
                var parts = duration.Split(':');
                
                if (parts.Length == 2)
                {
                    if (int.TryParse(parts[0], out int first) && int.TryParse(parts[1], out int second))
                    {
                        if (first > 59)
                        {
                            return first * 3600 + second * 60;
                        }
                        else
                        {
                            return first * 60 + second;
                        }
                    }
                }
                else if (parts.Length == 3)
                {
                    if (int.TryParse(parts[0], out int hours) && 
                        int.TryParse(parts[1], out int minutes) && 
                        int.TryParse(parts[2], out int seconds))
                    {
                        if (minutes >= 0 && minutes < 60 && seconds >= 0 && seconds < 60)
                        {
                            return hours * 3600 + minutes * 60 + seconds;
                        }
                    }
                }
            }
            catch { }
            
            return 0;
        }
    }
}
