using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Music
{
    public static class Img
    {
        public static BitmapImage GetImage(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return GetPlaceholder();

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                bitmap.DownloadFailed += (sender, e) =>
                {
                    (sender as BitmapImage)!.UriSource = new Uri(@"/Resources/placeholder_cover.png", UriKind.RelativeOrAbsolute);
                };

                return bitmap;
            }
            catch
            {
                return GetPlaceholder();
            }
        }

        private static BitmapImage GetPlaceholder()
        {
            return new BitmapImage(new Uri(@"/Resources/placeholder_cover.png", UriKind.RelativeOrAbsolute));
        }
    }

}
