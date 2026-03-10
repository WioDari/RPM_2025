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
    /// Логика взаимодействия для OpenAlbumWindow.xaml
    /// </summary>
    public partial class OpenAlbumWindow : Window
    {
        public OpenAlbumWindow()
        {
            InitializeComponent();
        }
        public OpenAlbumWindow(Album album, User user)
        {
            InitializeComponent();
            DataContext = album;

            if (user.RoleId != 1)
            {
                EditButton.Visibility = Visibility.Hidden;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditAlbumWindow editAlbumWindow = new((DataContext as Album)!.Id) { Owner = this.Owner };
            editAlbumWindow.ShowDialog();
            Close();
        }
    }
}
