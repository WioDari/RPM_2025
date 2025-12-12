using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Music.Views.Converters
{
    public class BackgroundMainBtnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string selectedButton && parameter is string buttonName)
            {
                return selectedButton == buttonName
                    ? new BrushConverter().ConvertFromString("#9C4DFF") 
                    : new BrushConverter().ConvertFromString("#0C000000");
            }
            return new BrushConverter().ConvertFromString("#0C000000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    
    }
}
