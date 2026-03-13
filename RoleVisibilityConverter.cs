using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spotify_wpf.Models;
using System.Windows.Data;
using System.Windows;

namespace Spotify_wpf
{
    public class RoleVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            
            if (Application.Current.Properties["CurrentUser"] is not User user)
                return Visibility.Collapsed;



            if (parameter == null)
                return Visibility.Collapsed;

            string requiredRole = parameter.ToString();

            return user.Role.Role1 == requiredRole ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
