using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Music
{
    public static class Img
    {
        public static BitmapImage GetImage(string? uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return GetPlaceholder();

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.DecodePixelWidth = 300;
                bitmap.DecodePixelHeight = 300;
                bitmap.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.None;
                bitmap.EndInit();

                bitmap.DownloadFailed += (sender, _) =>
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