using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Text;

public class Captcha
{
    private readonly Random _random = new Random();
    
    public (string Text, IImage Image) GenerateCaptcha(int width = 200, int height = 80)
    {
        // Генерируем случайный текст
        string captchaText = GenerateRandomText(6);
        
        // Создаём RenderTargetBitmap
        var bitmap = new RenderTargetBitmap(new PixelSize(width, height));
        
        using (var ctx = bitmap.CreateDrawingContext())
        {
            // Фон
            ctx.FillRectangle(
                new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                new Rect(0, 0, width, height));
            
            // Шум - точки
            for (int i = 0; i < 100; i++)
            {
                var brush = new SolidColorBrush(
                    Color.FromArgb(50, 
                        (byte)_random.Next(256), 
                        (byte)_random.Next(256), 
                        (byte)_random.Next(256)));
                
                ctx.DrawRectangle(
                    brush,
                    null,
                    new Rect(_random.Next(width), _random.Next(height), 1, 1));
            }
            
            // Текст
            for (int i = 0; i < captchaText.Length; i++)
            {
                var text = new FormattedText(
                    captchaText[i].ToString(),
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial", FontStyle.Normal, FontWeight.Bold),
                    36,
                    Brushes.Black);
                
                double x = 20 + i * 30;
                double y = 20 + _random.Next(-5, 5);
                
                ctx.DrawText(text, new Point(x, y));
            }
            
            // Помехи - линии
            for (int i = 0; i < 3; i++)
            {
                var pen = new Pen(
                    new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)));
                
                ctx.DrawLine(
                    pen,
                    new Point(_random.Next(width), _random.Next(height)),
                    new Point(_random.Next(width), _random.Next(height)));
            }
        }
        
        return (captchaText, bitmap);
    }
    
    private string GenerateRandomText(int length)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var sb = new StringBuilder();
        
        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[_random.Next(chars.Length)]);
        }
        
        return sb.ToString();
    }
}