using Musick.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Musick.Converters
{
    class PremiumUserConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string ID)
            {
                MusicContext context = new MusicContext();
                var hh = context.Playlists.FirstOrDefault(x => x.PlaylistId == int.Parse(ID));
                var jj = context.Users.FirstOrDefault(x => x.UserId == hh.UserId);


                if (jj.SubscriptionId == 2) 
                {
                    return (Brush)new BrushConverter().ConvertFromString("#ff943d");
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
