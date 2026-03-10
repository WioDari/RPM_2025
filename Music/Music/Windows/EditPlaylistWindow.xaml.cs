using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
using Music.UserControls;
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
    /// Логика взаимодействия для EditPlaylistWindow.xaml
    /// </summary>
    public partial class EditPlaylistWindow : Window
    {
        private MusicContext _context = null!;
        private Playlist _playlist = null!;

        public EditPlaylistWindow()
        {
            InitializeComponent();
        }

        public EditPlaylistWindow(int playlistId)
        {
            InitializeComponent();
            Load(playlistId);
        }

        private async void Load(int playlistId)
        {
            _context = new();

            _playlist = await _context.Playlists.Include(x => x.Tracks).Include(x => x.CreatorUser).Include(x => x.Tags).FirstAsync(x => x.Id == playlistId);

            DataContext = _playlist;

            TrackBox.ItemsSource = await _context.Tracks.ToListAsync();
            TagBox.ItemsSource = await _context.Tags.ToListAsync();

            CreatorBox.ItemsSource = await _context.Users.ToListAsync();
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

                if ((CreatorBox.SelectedItem as User) == null)
                    sb.AppendLine("Укажите создателя плейлиста");

                if (CreatedDateTimePicker.SelectedDate == null)
                    sb.AppendLine("Укажите дату создания");

                if (sb.ToString() != "Неверно указаны данные:\n")
                {
                    MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _playlist.Name = NameBox.Text;
                _playlist.CreatorUserId = (CreatorBox.SelectedItem as User).Id;
                _playlist.CreatedDateTime = CreatedDateTimePicker.SelectedDate; 
                _playlist.Description = DescBox.Text;

                await _context.SaveChangesAsync();

                MessageBox.Show($"Плейлист {_playlist.Name} был успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                ((Owner as MenuWindow)!.PlaylistsUC)!.Load();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Некорректный ввод данных\n{ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
            _playlist.Tracks.Remove(button.DataContext as Models.Track);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить этот плейлист?\nВсе треки будут сохранены.", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            _context.Playlists.Remove(_playlist);
            await _context.SaveChangesAsync();

            MessageBox.Show($"Альбом {_playlist.Name} был успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            ((Owner as MenuWindow)!.PlaylistsUC as PlaylistsUserControl)!.Load();

            Close();
            
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
            var button = (sender as Button);
            _playlist.Tags.Remove(button.DataContext as Tag);
        }
    }
}
