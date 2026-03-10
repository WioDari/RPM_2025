using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Music.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddPlaylistWindow.xaml
    /// </summary>
    public partial class AddPlaylistWindow : Window
    {
        private Playlist _playlist = null!;
        private MusicContext _context = null!;

        public AddPlaylistWindow()
        {
            InitializeComponent();
        }

        public AddPlaylistWindow(User user)
        {
            InitializeComponent();
            Loaded += (_, _) => Load(user);
        }

        private async void Load(User user)
        {
            _playlist = new Playlist() { CreatorUserId = user.Id, CreatedDateTime = DateTime.Now };
            _context = new MusicContext();

            DataContext = _playlist;

            TrackBox.ItemsSource = await _context.Tracks.ToListAsync();
            TagBox.ItemsSource = await _context.Tags.ToListAsync();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreatorBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var popup = (Popup)CreatorBox.Template.FindName("PART_Popup", CreatorBox);
            if (popup != null)
            {
                popup.IsOpen = true;
            }
        }

        private void CreatorBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var popup = (Popup)CreatorBox.Template.FindName("PART_Popup", CreatorBox);
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder sb = new("Неверно указаны данные:\n");
                if (string.IsNullOrEmpty(NameBox.Text))
                    sb.AppendLine("Укажите название плейлиста");

                if (DescBox.Text == null)
                    DescBox.Text = "";

                //if ((CreatorBox.SelectedItem as User) == null)
                  //  sb.AppendLine("Укажите создателя плейлиста");

                if (CreatedDateTimePicker.SelectedDate == null)
                    sb.AppendLine("Укажите дату создания");

                if (sb.ToString() != "Неверно указаны данные:\n")
                {
                    MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _playlist.Id = await _context.Playlists.AnyAsync() ? await _context.Playlists.MaxAsync(x => x.Id) + 1 : 1;
                _playlist.Name = NameBox.Text;
                //_playlist.CreatorUserId = (CreatorBox.SelectedItem as User)!.Id;
                _playlist.CreatedDateTime = CreatedDateTimePicker.SelectedDate;
                _playlist.Description = DescBox.Text;

                await _context.Playlists.AddAsync(_playlist);

                await _context.SaveChangesAsync();

                MessageBox.Show($"Плейлист {_playlist.Name} был успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Некорректный ввод данных\n{ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void AddTrackButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTrack = TrackBox.SelectedItem as Models.Track;

            if (selectedTrack != null && !_playlist.Tracks.Contains(selectedTrack))
            {
                _playlist.Tracks.Add(selectedTrack);
            }
        }

        private void RemoveTrack_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button)!;
            _playlist.Tracks.Remove((button.DataContext as Models.Track)!);
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTag = TagBox.SelectedItem as Tag;

            if (selectedTag != null && !_playlist.Tags.Contains(selectedTag))
            {
                _playlist.Tags.Add(selectedTag);
            }
        }

        private void RemoveTag_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button)!;
            _playlist.Tags.Remove((button.DataContext as Tag)!);
        }
    }
}
