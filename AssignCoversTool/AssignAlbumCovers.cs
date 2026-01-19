using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MusicPlus
{
    public class AlbumCoverAssigner
    {
        private readonly ApplicationDbContext _context;
        private readonly string _coversDirectory;

        public AlbumCoverAssigner(ApplicationDbContext context, string coversDirectory = "Resources/covers")
        {
            _context = context;
            _coversDirectory = coversDirectory;
        }

        public void AssignCovers(bool useBinary = false)
        {
            Console.WriteLine("=== Привязка обложек альбомов ===");
            Console.WriteLine();

            if (!Directory.Exists(_coversDirectory))
            {
                Console.WriteLine($"Ошибка: Папка '{_coversDirectory}' не найдена!");
                return;
            }

            var imageFiles = Directory.GetFiles(_coversDirectory, "*.jpg")
                .Concat(Directory.GetFiles(_coversDirectory, "*.jpeg"))
                .Concat(Directory.GetFiles(_coversDirectory, "*.png"))
                .ToArray();

            Console.WriteLine($"Найдено файлов обложек: {imageFiles.Length}");
            Console.WriteLine();

            int successCount = 0;
            int notFoundCount = 0;
            int errorCount = 0;

            var albums = _context.Albums.ToList();

            foreach (var imageFile in imageFiles)
            {
                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(imageFile);
                    string fileNameWithExt = Path.GetFileName(imageFile);

                    var album = albums.FirstOrDefault(a => 
                        a.AlbumTitle.Equals(fileName, StringComparison.OrdinalIgnoreCase));

                    if (album == null)
                    {
                        album = albums.FirstOrDefault(a => 
                            fileName.Contains(a.AlbumTitle, StringComparison.OrdinalIgnoreCase) ||
                            a.AlbumTitle.Contains(fileName, StringComparison.OrdinalIgnoreCase));
                    }

                    if (album == null)
                    {
                        Console.WriteLine($"⚠ Альбом не найден для файла: {fileNameWithExt}");
                        notFoundCount++;
                        continue;
                    }

                    if (useBinary)
                    {
                        byte[] imageBytes = File.ReadAllBytes(imageFile);
                        album.CoverBinary = imageBytes;
                        album.CoverPath = null;
                        Console.WriteLine($"✓ Загружена обложка (бинарные данные) для: {album.AlbumTitle}");
                    }
                    else
                    {
                        album.CoverPath = fileNameWithExt;
                        album.CoverBinary = null;
                        Console.WriteLine($"✓ Привязан путь обложки для: {album.AlbumTitle} -> {fileNameWithExt}");
                    }

                    _context.SaveChanges();
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Ошибка при обработке файла {Path.GetFileName(imageFile)}: {ex.Message}");
                    errorCount++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("=== Результаты ===");
            Console.WriteLine($"Успешно привязано: {successCount}");
            Console.WriteLine($"Альбомы не найдены: {notFoundCount}");
            Console.WriteLine($"Ошибок: {errorCount}");
        }

        public void ShowMismatches()
        {
            Console.WriteLine("=== Анализ соответствий обложек и альбомов ===");
            Console.WriteLine();

            if (!Directory.Exists(_coversDirectory))
            {
                Console.WriteLine($"Ошибка: Папка '{_coversDirectory}' не найдена!");
                return;
            }

            var imageFiles = Directory.GetFiles(_coversDirectory, "*.jpg")
                .Concat(Directory.GetFiles(_coversDirectory, "*.jpeg"))
                .Concat(Directory.GetFiles(_coversDirectory, "*.png"))
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .ToList();

            var albums = _context.Albums.ToList();

            Console.WriteLine("Файлы обложек без соответствующих альбомов:");
            int orphanFiles = 0;
            foreach (var fileName in imageFiles)
            {
                bool found = albums.Any(a => 
                    a.AlbumTitle.Equals(fileName, StringComparison.OrdinalIgnoreCase) ||
                    fileName.Contains(a.AlbumTitle, StringComparison.OrdinalIgnoreCase) ||
                    a.AlbumTitle.Contains(fileName, StringComparison.OrdinalIgnoreCase));

                if (!found)
                {
                    Console.WriteLine($"  - {fileName}");
                    orphanFiles++;
                }
            }
            if (orphanFiles == 0)
            {
                Console.WriteLine("  (нет)");
            }

            Console.WriteLine();

            Console.WriteLine("Альбомы без обложек:");
            int albumsWithoutCovers = 0;
            foreach (var album in albums)
            {
                bool hasCoverFile = imageFiles.Any(f => 
                    f.Equals(album.AlbumTitle, StringComparison.OrdinalIgnoreCase) ||
                    f.Contains(album.AlbumTitle, StringComparison.OrdinalIgnoreCase) ||
                    album.AlbumTitle.Contains(f, StringComparison.OrdinalIgnoreCase));

                bool hasCoverInDb = !string.IsNullOrEmpty(album.CoverPath) || 
                                   (album.CoverBinary != null && album.CoverBinary.Length > 0);

                if (!hasCoverFile && !hasCoverInDb)
                {
                    Console.WriteLine($"  - {album.AlbumTitle} (ID: {album.AlbumID})");
                    albumsWithoutCovers++;
                }
            }
            if (albumsWithoutCovers == 0)
            {
                Console.WriteLine("  (нет)");
            }

            Console.WriteLine();
            Console.WriteLine($"Всего файлов обложек: {imageFiles.Count}");
            Console.WriteLine($"Всего альбомов: {albums.Count}");
        }
    }
}
