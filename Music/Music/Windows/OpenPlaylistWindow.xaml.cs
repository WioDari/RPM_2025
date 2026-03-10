using Music.Models;
using Music.UserControls;
using System;
using System.Collections.Generic;
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

namespace Music.Windows
{
    /// <summary>
    /// Логика взаимодействия для OpenPlaylistWindow.xaml
    /// </summary>
    public partial class OpenPlaylistWindow : Window
    {
        public OpenPlaylistWindow()
        {
            InitializeComponent();
        }

        public OpenPlaylistWindow(Playlist playlist, User user)
        {
            InitializeComponent();
            DataContext = playlist;
            EditButton.Visibility = Visibility.Hidden;
            if (playlist.CreatorUserId == user.Id || user.RoleId == 1)
                EditButton.Visibility = Visibility.Visible;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditPlaylistWindow editPlaylistWindow = new((DataContext as Playlist)!.Id) { Owner = this.Owner };
            Close();
            editPlaylistWindow.ShowDialog();
        }
    }
}
