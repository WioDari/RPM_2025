using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Data;
using MusicPlus.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using OfficeOpenXml;

namespace MusicPlus
{
    public class ExcelTrackImporter
    {
        private readonly ApplicationDbContext _context;

        public ExcelTrackImporter(ApplicationDbContext context)
        {
            _context = context;
        }

        public void ImportTracksFromExcel(string excelFilePath)
        {
            Console.WriteLine("Начало импорта треков из Excel...");
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension?.Rows ?? 0;
                
                if (rowCount < 2)
                {
                    Console.WriteLine("Файл пуст или содержит только заголовки!");
                    return;
                }

                Console.WriteLine($"Найдено строк: {rowCount - 1} (без заголовка)");

                int successCount = 0;
                int errorCount = 0;

                int headerRow = 1;
                var columnMap = GetColumnMap(worksheet, headerRow);

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var trackData = ReadTrackRow(worksheet, row, columnMap);
                        if (trackData != null)
                        {
                            SaveTrack(trackData);
                            successCount++;
                            
                            if (successCount % 100 == 0)
                            {
                                Console.WriteLine($"Обработано: {successCount} треков...");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        Console.WriteLine($"Ошибка в строке {row}: {ex.Message}");
                    }
                }

                Console.WriteLine($"\nИмпорт завершен!");
                Console.WriteLine($"Успешно: {successCount}");
                Console.WriteLine($"Ошибок: {errorCount}");
            }
        }

        private Dictionary<string, int> GetColumnMap(ExcelWorksheet worksheet, int headerRow)
        {
            var map = new Dictionary<string, int>();
            
            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
            {
                string header = worksheet.Cells[headerRow, col].Text?.Trim() ?? "";
                
                if (!string.IsNullOrEmpty(header))
                {
                    string normalized = header.ToLowerInvariant();
                    map[normalized] = col;
                }
            }

            return map;
        }

        private TrackData? ReadTrackRow(ExcelWorksheet worksheet, int row, Dictionary<string, int> columnMap)
        {
            try
            {
                string? trackName = GetCellValue(worksheet, row, "trackname", columnMap);
                string? artistName = GetCellValue(worksheet, row, "artistname", columnMap);
                string? albumInfo = GetCellValue(worksheet, row, "albuminfo", columnMap);
                string? genres = GetCellValue(worksheet, row, "genres", columnMap);
                string? duration = GetCellValue(worksheet, row, "duration", columnMap);
                string? releaseDate = GetCellValue(worksheet, row, "releasedate", columnMap);
                string? bitrate = GetCellValue(worksheet, row, "bitrate", columnMap);
                string? filePath = GetCellValue(worksheet, row, "filepath", columnMap);
                string? albumCoverPath = GetCellValue(worksheet, row, "albumcoverpath", columnMap);
                string? albumCoverBinary = GetCellValue(worksheet, row, "albumcoverbinary", columnMap);
                string? rating = GetCellValue(worksheet, row, "rating", columnMap);
                string? playCount = GetCellValue(worksheet, row, "playcount", columnMap);

                if (string.IsNullOrWhiteSpace(trackName) || string.IsNullOrWhiteSpace(artistName))
                {
                    return null;
                }

                return new TrackData
                {
                    TrackName = trackName?.Trim() ?? "",
                    ArtistName = artistName?.Trim() ?? "",
                    AlbumInfo = albumInfo?.Trim(),
                    Genres = genres?.Trim(),
                    Duration = ParseDuration(duration),
                    ReleaseDate = ParseDate(releaseDate),
                    Bitrate = ParseInt(bitrate),
                    FilePath = filePath?.Trim(),
                    AlbumCoverPath = ProcessCoverPath(albumCoverPath),
                    AlbumCoverBinaryStr = albumCoverBinary,
                    Rating = ParseDecimal(rating),
                    PlayCount = ParseInt(playCount) ?? 0
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка чтения строки {row}: {ex.Message}", ex);
            }
        }

        private string? GetCellValue(ExcelWorksheet worksheet, int row, string columnKey, Dictionary<string, int> columnMap)
        {
            if (columnKey == null || !columnMap.ContainsKey(columnKey.ToLowerInvariant()))
                return null;
            
            int col = columnMap[columnKey.ToLowerInvariant()];
            var cellValue = worksheet.Cells[row, col].Value;
            
            if (cellValue == null)
                return null;
                
            return cellValue.ToString()?.Trim();
        }

        private string? ProcessCoverPath(string? coverPath)
        {
            if (string.IsNullOrWhiteSpace(coverPath))
                return null;

            string trimmed = coverPath.Trim();

            if (trimmed.Length > 500)
            {
                Console.WriteLine($"Внимание: AlbumCoverPath длиннее 500 символов, будет использован CoverBinary");
                return null;
            }

            return trimmed;
        }


        private int ParseDuration(string? durationStr)
        {
            if (string.IsNullOrWhiteSpace(durationStr))
                return 0;

            durationStr = durationStr.Trim();

            if (durationStr.Contains(':'))
            {
                var parts = durationStr.Split(':');
                if (parts.Length == 2)
                {
                    if (int.TryParse(parts[0], out int minutes) && 
                        int.TryParse(parts[1], out int seconds))
                    {
                        return minutes * 60 + seconds;
                    }
                }
                else if (parts.Length == 3)
                {
                    if (int.TryParse(parts[0], out int hours) &&
                        int.TryParse(parts[1], out int minutes) &&
                        int.TryParse(parts[2], out int seconds))
                    {
                        return hours * 3600 + minutes * 60 + seconds;
                    }
                }
            }
            else if (int.TryParse(durationStr, out int seconds))
            {
                return seconds;
            }

            return 0;
        }

        private DateTime? ParseDate(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
                return null;

            dateStr = dateStr.Trim();

            string[] formats = {
                "yyyy-MM-dd",
                "dd.MM.yyyy",
                "MM/dd/yyyy",
                "dd/MM/yyyy",
                "yyyy/MM/dd"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(dateStr, format, CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }

            if (DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out DateTime defaultResult))
            {
                return defaultResult;
            }

            return null;
        }

        private int? ParseInt(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (int.TryParse(value.Trim(), out int result))
                return result;

            return null;
        }

        private decimal? ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (decimal.TryParse(value.Trim().Replace(',', '.'), 
                NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
                return result;

            return null;
        }

        private void SaveTrack(TrackData trackData)
        {
            var artist = _context.Artists
                .FirstOrDefault(a => a.ArtistName == trackData.ArtistName);

            if (artist == null)
            {
                artist = new Artist
                {
                    ArtistName = trackData.ArtistName,
                    Country = "Unknown"
                };
                _context.Artists.Add(artist);
                _context.SaveChanges();
            }

            int? albumId = null;
            if (!string.IsNullOrWhiteSpace(trackData.AlbumInfo))
            {
                var album = _context.Albums
                    .Include(a => a.Artist)
                    .FirstOrDefault(a => a.AlbumTitle == trackData.AlbumInfo && 
                                       a.ArtistID == artist.ArtistID);

                if (album == null)
                {
                    album = new Album
                    {
                        AlbumTitle = trackData.AlbumInfo,
                        ArtistID = artist.ArtistID,
                        ReleaseYear = trackData.ReleaseDate?.Year,
                        TotalDuration = 0
                    };
                    _context.Albums.Add(album);
                    _context.SaveChanges();
                }
                albumId = album.AlbumID;
            }

            var connection = _context.Database.GetDbConnection();
            connection.Open();
            
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO Tracks 
                    (TrackName, Duration, ReleaseDate, Bitrate, FilePath, AlbumCoverPath, AlbumCoverBinary, Rating, PlayCount, AlbumID)
                    VALUES 
                    (@trackName, @duration, @releaseDate, @bitrate, @filePath, @albumCoverPath, @albumCoverBinary, @rating, @playCount, @albumId)";
                
                var paramTrackName = command.CreateParameter();
                paramTrackName.ParameterName = "@trackName";
                paramTrackName.Value = trackData.TrackName;
                command.Parameters.Add(paramTrackName);
                
                var paramDuration = command.CreateParameter();
                paramDuration.ParameterName = "@duration";
                paramDuration.Value = trackData.Duration;
                command.Parameters.Add(paramDuration);
                
                var paramReleaseDate = command.CreateParameter();
                paramReleaseDate.ParameterName = "@releaseDate";
                paramReleaseDate.Value = (object?)trackData.ReleaseDate ?? DBNull.Value;
                command.Parameters.Add(paramReleaseDate);
                
                var paramBitrate = command.CreateParameter();
                paramBitrate.ParameterName = "@bitrate";
                paramBitrate.Value = (object?)trackData.Bitrate ?? DBNull.Value;
                command.Parameters.Add(paramBitrate);
                
                var paramFilePath = command.CreateParameter();
                paramFilePath.ParameterName = "@filePath";
                paramFilePath.Value = (object?)trackData.FilePath ?? DBNull.Value;
                command.Parameters.Add(paramFilePath);
                
                var paramAlbumCoverPath = command.CreateParameter();
                paramAlbumCoverPath.ParameterName = "@albumCoverPath";
                paramAlbumCoverPath.Value = (object?)trackData.AlbumCoverPath ?? DBNull.Value;
                command.Parameters.Add(paramAlbumCoverPath);
                
                var paramAlbumCoverBinary = command.CreateParameter();
                paramAlbumCoverBinary.ParameterName = "@albumCoverBinary";
                paramAlbumCoverBinary.Value = (object?)trackData.AlbumCoverBinaryStr ?? DBNull.Value;
                command.Parameters.Add(paramAlbumCoverBinary);
                
                var paramRating = command.CreateParameter();
                paramRating.ParameterName = "@rating";
                paramRating.Value = (object?)trackData.Rating ?? DBNull.Value;
                command.Parameters.Add(paramRating);
                
                var paramPlayCount = command.CreateParameter();
                paramPlayCount.ParameterName = "@playCount";
                paramPlayCount.Value = trackData.PlayCount;
                command.Parameters.Add(paramPlayCount);
                
                var paramAlbumId = command.CreateParameter();
                paramAlbumId.ParameterName = "@albumId";
                paramAlbumId.Value = (object?)albumId ?? DBNull.Value;
                command.Parameters.Add(paramAlbumId);
                
                command.ExecuteNonQuery();
                
                command.CommandText = "SELECT LAST_INSERT_ID()";
                int trackId = Convert.ToInt32(command.ExecuteScalar());
                
                command.CommandText = @"
                    INSERT INTO TrackArtists (TrackID, ArtistID)
                    VALUES (@trackId, @artistId)";
                command.Parameters.Clear();
                
                var paramTrackId = command.CreateParameter();
                paramTrackId.ParameterName = "@trackId";
                paramTrackId.Value = trackId;
                command.Parameters.Add(paramTrackId);
                
                var paramArtistId = command.CreateParameter();
                paramArtistId.ParameterName = "@artistId";
                paramArtistId.Value = artist.ArtistID;
                command.Parameters.Add(paramArtistId);
                
                command.ExecuteNonQuery();
            }
            
            connection.Close();

            if (!string.IsNullOrWhiteSpace(trackData.Genres))
            {
                var genreNames = trackData.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(g => g.Trim())
                    .Where(g => !string.IsNullOrWhiteSpace(g));

                foreach (var genreName in genreNames)
                {
                    var genre = _context.Genres.FirstOrDefault(g => g.GenreName == genreName);
                    if (genre == null)
                    {
                        genre = new Genre { GenreName = genreName };
                        _context.Genres.Add(genre);
                        _context.SaveChanges();
                    }

                    if (albumId.HasValue)
                    {
                        var albumGenreExists = _context.AlbumGenres
                            .Any(ag => ag.AlbumID == albumId.Value && ag.GenreID == genre.GenreID);

                        if (!albumGenreExists)
                        {
                            var albumGenre = new AlbumGenre
                            {
                                AlbumID = albumId.Value,
                                GenreID = genre.GenreID
                            };
                            _context.AlbumGenres.Add(albumGenre);
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }

            private class TrackData
        {
            public string TrackName { get; set; } = "";
            public string ArtistName { get; set; } = "";
            public string? AlbumInfo { get; set; }
            public string? Genres { get; set; }
            public int Duration { get; set; }
            public DateTime? ReleaseDate { get; set; }
            public int? Bitrate { get; set; }
            public string? FilePath { get; set; }
            public string? AlbumCoverPath { get; set; }
            public string? AlbumCoverBinaryStr { get; set; }
            public decimal? Rating { get; set; }
            public int PlayCount { get; set; }
        }
    }
}
