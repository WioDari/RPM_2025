using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Music
{
    static public class ImgHttp
    {
        public static ImageSource GetImage(string? uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return GetPlaceholder();

            try
            {
                byte[] imageData;
                using (var client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/120.0.0.0 Safari/537.36");
                    imageData = client.DownloadData(uri);
                }

                using (var ms = new MemoryStream(imageData))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;

                    // Ограничиваем размер декодирования для экономии RAM
                    bitmap.DecodePixelWidth = 300;

                    bitmap.EndInit();

                    // Замораживаем объект: это дает +30% к скорости рендеринга 
                    // и позволяет использовать картинку в разных потоках
                    bitmap.Freeze();

                    // Если все же нужна обрезка до квадрата (Crop)
                    return CreateSquareCroppedBitmap(bitmap);
                }
            }
            catch
            {
                return GetPlaceholder();
            }
        }

        private static ImageSource CreateSquareCroppedBitmap(BitmapSource bitmap)
        {
            int size = Math.Min(bitmap.PixelWidth, bitmap.PixelHeight);
            int x = (bitmap.PixelWidth - size) / 2;
            int y = (bitmap.PixelHeight - size) / 2;

            var cropped = new CroppedBitmap(bitmap, new Int32Rect(x, y, size, size));
            cropped.Freeze(); // Тоже замораживаем
            return cropped;
        }

        public static ImageSource GetPlaceholder()
        {
            // Используем pack-синтаксис для надежности ресурсов
            var bitmap = new BitmapImage(new Uri("pack://application:,,,/Resources/placeholder_cover.png", UriKind.Absolute));
            if (!bitmap.IsFrozen) bitmap.Freeze();
            return bitmap;
        }
    
    }
}
