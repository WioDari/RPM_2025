using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MusicPlus
{
    class AssignCoversProgram
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8; 

            Console.WriteLine("=== Утилита привязки обложек альбомов ===");
            Console.WriteLine();

            string coversDirectory = "Resources/covers";
            
            if (!Directory.Exists(coversDirectory))
            {
                Console.WriteLine($"Ошибка: Папка '{coversDirectory}' не найдена!");
                Console.WriteLine("Проверьте путь к папке с обложками.");
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

                    var assigner = new AlbumCoverAssigner(context, coversDirectory);

                    while (true)
                    {
                        Console.WriteLine("\nВыберите действие:");
                        Console.WriteLine("1. Показать анализ соответствий (файлы без альбомов и альбомы без обложек)");
                        Console.WriteLine("2. Привязать обложки (сохранить пути к файлам)");
                        Console.WriteLine("3. Загрузить обложки в БД (бинарные данные)");
                        Console.WriteLine("4. Выход");
                        Console.Write("\nВаш выбор: ");

                        string? choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine();
                                assigner.ShowMismatches();
                                break;

                            case "2":
                                Console.WriteLine();
                                Console.WriteLine("Привязка обложек по путям к файлам...");
                                assigner.AssignCovers(useBinary: false);
                                break;

                            case "3":
                                Console.WriteLine();
                                Console.WriteLine("Загрузка обложек в БД (бинарные данные)...");
                                Console.WriteLine("Внимание: это может занять время и увеличить размер БД!");
                                Console.Write("Продолжить? (y/n): ");
                                string? confirm = Console.ReadLine();
                                if (confirm?.ToLower() == "y" || confirm?.ToLower() == "yes" || confirm?.ToLower() == "д")
                                {
                                    assigner.AssignCovers(useBinary: true);
                                }
                                else
                                {
                                    Console.WriteLine("Операция отменена.");
                                }
                                break;

                            case "4":
                                Console.WriteLine("Выход...");
                                return;

                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                    }
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
