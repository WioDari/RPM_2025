using Microsoft.EntityFrameworkCore;
using Music.Context;
using Music.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Music.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UsersUserControl.xaml
    /// </summary>
    public partial class UsersUserControl : UserControl
    {
        private MusicContext _context = new();
        private ObservableCollection<User> _users = new();
        private Window _owner = null!;

        public UsersUserControl()
        {
            InitializeComponent();
        }

        public UsersUserControl(Window owner)
        {
            InitializeComponent();
            UsersListBox.ItemsSource = _users;
            _owner = owner;
            Loaded += (_, _) => Load();
        }

        private async void Load()
        {
            _users.Clear();
            var users = await _context.Users.Include(x => x.Role).Include(x => x.Subscription).OrderBy(x => x.Login).ToListAsync();

            foreach (var user in users)
            {
                _users.Add(user);
            }
        }

        private async void UnbanUserButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button)!;
            button.IsEnabled = false;
            _context = new();
            var user = await _context.Users.FirstAsync(x => x.Id == (button.DataContext as User)!.Id);
            user.BanInterval = TimeSpan.Zero;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            MessageBox.Show(_owner, $"Пользователь {user.Login} разблокирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
            button.IsEnabled = true;
            Load();
        }

        private async void BanUserButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button)!;
            button.IsEnabled = false;
            _context = new();
            var user = await _context.Users.FirstAsync(x => x.Id == (button.DataContext as User)!.Id);
            user.BanInterval = TimeSpan.FromMinutes(60);
            user.BanDateTime = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            MessageBox.Show(_owner, $"Пользователь {user.Login} заблокирован на час.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
            button.IsEnabled = true;
            Load();
        } 
    }
}

