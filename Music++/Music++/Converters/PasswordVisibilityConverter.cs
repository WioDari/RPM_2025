using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Music__.Converters
{
    /*
    public class PasswordVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targettype, object parameter, CultureInfo culture)
        {
            if (value is string password)
            {
                bool ShowPassword = Application.Current.Properties["IsChecked"] == null ? false : (bool)Application.Current.Properties["IsChecked"];
                if (ShowPassword)
                {
                    return value;
                }
                else
                {
                    return new string('*', password.Length);
                }
            }
            return value;
        }
        
        public object ConvertBack(object value, Type targettype, object parameter, CultureInfo culture)
        {
            return value as string;
        }
        
    }
    */
}
