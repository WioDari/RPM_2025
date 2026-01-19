using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicPlus.Converters
{
    public class ActiveMenuButtonConverter : IValueConverter
    {
        private static readonly Color LightPurpleColor = (Color)ColorConverter.ConvertFromString("#9C4DFF");
        private static readonly Color DarkPurpleColor = (Color)ColorConverter.ConvertFromString("#7A00CC");
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                string buttonName = parameter?.ToString() ?? "";
                
                if (isActive)
                {
                    return new SolidColorBrush(LightPurpleColor);
                }
                
                if (buttonName == "Артисты" || buttonName == "Плейлисты")
                {
                    return new SolidColorBrush(DarkPurpleColor);
                }
                
                return new SolidColorBrush(DarkPurpleColor);
            }
            return new SolidColorBrush(DarkPurpleColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
