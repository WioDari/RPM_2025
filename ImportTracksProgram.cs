using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MusicPlus
{
    class ImportTracksProgram
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Импорт треков из Excel ===");
            Console.WriteLine();

            string excelFilePath = "spotify_database.xlsx";

            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine($"Ошибка: Файл '{excelFilePath}' не найден!");
                Console.WriteLine("Поместите файл Excel в ту же папку, что и программа.");
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
                return;
            }

            try
            {
                var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
                var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseMySql(connectionString, serverVersion)
                    .Options;

                using (var context = new ApplicationDbContext(options))
                {
                    if (!context.Database.CanConnect())
                    {
                        Console.WriteLine("Ошибка: Не удалось подключиться к базе данных!");
                        Console.WriteLine("Проверьте, что MySQL запущен и база данных создана.");
                        Console.WriteLine("\nНажмите любую клавишу для выхода...");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("✓ Подключение к базе данных установлено");
                    Console.WriteLine();

                    var importer = new ExcelTrackImporter(context);
                    importer.ImportTracksFromExcel(excelFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОШИБКА: {ex.Message}");
                Console.WriteLine($"Детали: {ex.StackTrace}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
