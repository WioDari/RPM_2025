using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using MusicPlus.Models;
using MusicPlus.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MusicPlus.Views
{
    public partial class AddTracksToPlaylistView : Window
    {
        private int _playlistId;
        private ApplicationDbContext _context;
        private List<PlaylistAddTrackViewModel> _allTracks = new List<PlaylistAddTrackViewModel>();

        public AddTracksToPlaylistView(int playlistId)
        {
            InitializeComponent();
            _playlistId = playlistId;

            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;

            _context = new ApplicationDbContext(options);

            LoadTracks();
        }

        private void LoadTracks()
        {
            try
            {

                var tracks = _context.Tracks
                    .Include(t => t.TrackArtists)
                        .ThenInclude(ta => ta.Artist)
                    .Include(t => t.Album)
                    .OrderBy(t => t.TrackName)
                    .ToList();

                var existingTrackIds = _context.PlaylistTracks
                    .Where(pt => pt.PlaylistID == _playlistId)
                    .Select(pt => pt.TrackID)
                    .ToList();

                _allTracks.Clear();
                foreach (var track in tracks)
                {

                    if (existingTrackIds.Contains(track.TrackID))
                        continue;

                    var trackVm = new PlaylistAddTrackViewModel
                    {
                        TrackID = track.TrackID,
                        TrackName = track.TrackName,
                        Duration = track.Duration,
                        Rating = track.Rating ?? 0,
                        ArtistsText = GetTrackArtists(track),
                        AlbumTitle = track.Album?.AlbumTitle ?? ""
                    };
                    _allTracks.Add(trackVm);
                }

                TracksListView.ItemsSource = _allTracks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки треков: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetTrackArtists(Track track)
        {
            if (track.TrackArtists == null || !track.TrackArtists.Any())
                return "Не указано";

            return string.Join(", ", track.TrackArtists
                .Where(ta => ta.Artist != null)
                .Select(ta => ta.Artist!.ArtistName)
                .OrderBy(a => a));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedTracks = TracksListView.SelectedItems.Cast<PlaylistAddTrackViewModel>().ToList();
                
                if (selectedTracks.Count == 0)
                {
                    MessageBox.Show("Выберите хотя бы один трек для добавления", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                foreach (var trackVm in selectedTracks)
                {

                    var exists = _context.PlaylistTracks
                        .Any(pt => pt.PlaylistID == _playlistId && pt.TrackID == trackVm.TrackID);

                    if (!exists)
                    {
                        _context.PlaylistTracks.Add(new PlaylistTrack
                        {
                            PlaylistID = _playlistId,
                            TrackID = trackVm.TrackID,
                            DateAdded = DateTime.Now
                        });
                    }
                }

                _context.SaveChanges();
                MessageBox.Show($"Успешно добавлено треков: {selectedTracks.Count}", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления треков: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _context?.Dispose();
            base.OnClosing(e);
        }
    }

    public class PlaylistAddTrackViewModel
    {
        public int TrackID { get; set; }
        public string TrackName { get; set; } = "";
        public int Duration { get; set; }
        public decimal Rating { get; set; }
        public string ArtistsText { get; set; } = "";
        public string AlbumTitle { get; set; } = "";
    }
}
