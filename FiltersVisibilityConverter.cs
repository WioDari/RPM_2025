using System;
using System.Collections.Generic;
using System.Windows;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MusicWpf.Models;


namespace MusicWpf
{
    class FiltersVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Application.Current.Properties["CurrentUser"] == null)
            {
                return false;
            }
            var user = (User)Application.Current.Properties["CurrentUser"];
            if (user.RoleId == 3)
                return false;
            else
                return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
