using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MusicPlus.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Collapsed;

            string userRole = value.ToString() ?? "";
            string allowedRoles = parameter.ToString() ?? "";

            if (string.IsNullOrEmpty(allowedRoles))
                return Visibility.Visible;

            string[] roles = allowedRoles.Split('|');
            foreach (string role in roles)
            {
                if (userRole.Equals(role.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
