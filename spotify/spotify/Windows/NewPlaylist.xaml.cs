using spotify.Context;
using spotify.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace spotify.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewPlaylistWindow.xaml
    /// </summary>
    public partial class NewPlaylistWindow : Window
    {
        User user1 = new();
        Playlist playlist1;
        public SpotifyContext context;
        ObservableCollection<Tag> tags = [];
        ObservableCollection<Track> tracks = [];
        public NewPlaylistWindow()
        {
            InitializeComponent();
        }

        public NewPlaylistWindow(User user)
        {
            InitializeComponent();
            user1 = user;

            TagComboBox.ItemsSource = context.Tags.ToList();
            TrackComboBox.ItemsSource = context.Tracks.ToList();
        }

        public NewPlaylistWindow(Playlist playlist)
        {
            InitializeComponent();
            Title = "Редактирование плейлиста";
            TitleTextBox.Text = "Редактирование плейлиста";
            playlist1 = playlist;
            PlaylistNameTextBox.Text = playlist.PlaylistName;
            DescriptionTextBox.Text = playlist.Description;
            TagListBox.ItemsSource = playlist.Tags;
            TrackListBox.ItemsSource = playlist.Tracks;
            TagComboBox.ItemsSource = context.Tags.ToList();
            TrackComboBox.ItemsSource = context.Tracks.ToList();
        }

        private async void NewPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            var color = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            PlaylistNameTextBox.BorderBrush = color;
            TagListBox.BorderBrush = color;
            TrackListBox.BorderBrush = color;

            if (string.IsNullOrEmpty(PlaylistNameTextBox.Text) || tags.Count == 0 || tracks.Count == 0)
            {
                if(string.IsNullOrEmpty(PlaylistNameTextBox.Text))
                {
                    PlaylistNameTextBox.BorderBrush = Brushes.Red;
                }
                if(tags.Count == 0)
                {
                    TagListBox.BorderBrush = Brushes.Red;
                }
                if(tracks.Count == 0)
                {
                    TrackListBox.BorderBrush = Brushes.Red;
                }
                MessageBox.Show("Какие то из полей не заполенены");
                return;
            }

            var playlist = new Playlist()
            {
                Id = context.Playlists.Any() ? context.Playlists.Max(x => x.Id) + 1 : 1,
                PlaylistName = PlaylistNameTextBox.Text,
                UserId = user1.Id,
                DateCreated = DateOnly.FromDateTime(DateTime.Now),
                Likes = 0,
                Description = DescriptionTextBox.Text
            };

            context.Playlists.Add(playlist);
            context.SaveChanges();

            playlist.Tags = tags;
            context.Playlists.Update(playlist);
            playlist.Tracks = tracks;
            context.Playlists.Update(playlist);
            await context.SaveChangesAsync();

            MessageBox.Show("Плейлист успешно добавлен");
            return;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new(user1);
            mainMenuWindow.Show();
            this.Close();
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.IsDropDownOpen = true;
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.IsDropDownOpen = false;
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagComboBox.SelectedItem != null)
            {
                if (tags.Contains(TagComboBox.SelectedItem as Tag))
                {
                    MessageBox.Show("Тег уже добавлен");
                    return;
                }
                tags.Add(TagComboBox.SelectedItem as Tag);
                TagListBox.ItemsSource = tags;
                TagComboBox.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Тег не выбран");
                return;
            }
        }

        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            tags.Remove(button.DataContext as Tag);
        }

        private void AddTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrackComboBox.SelectedItem != null)
            {
                if (tracks.Contains(TrackComboBox.SelectedItem as Track))
                {
                    MessageBox.Show("Трек уже добавлен");
                    return;
                }
                tracks.Add(TrackComboBox.SelectedItem as Track);
                TrackListBox.ItemsSource = tracks;
                TrackComboBox.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Трек не выбран");
                return;
            }
        }

        private void DeleteTrack_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            tracks.Remove(button.DataContext as Track);
        }
    }
}
