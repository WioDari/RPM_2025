using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Spotify_wpf
{
    class CheckBoxChangeVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string passwordd)
            {
                bool showPassword = Application.Current.Properties["isChecked"] == null ? false : (bool)Application.Current.Properties["isChecked"];
                if (showPassword)
                {
                    return value;
                }
                else
                {
                    return new string('*' , passwordd.Length);
                }
                
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string;
        }
    }
}
