using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Spotify_wpf.Context;
using Spotify_wpf.Models;
using static Spotify_wpf.ViewModels.AlbumViewModel;

namespace Spotify_wpf.ViewModels
{
    class UserListViewModel : BaseViewModel
    {
        private ObservableCollection<ListUser> _listUser;

        private int _id { get; set; }
        public int id
        {
            get => _id;
            set
            {
                _id = value;
                selectedUser.Id = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ListUser> listUser
        {
            get => _listUser;

            set
            {
                _listUser = value;
                OnPropertyChanged();
               
            }
        }
        public ListUser selectedUser { get; set; }

        

        public UserListViewModel()
        {
            LoadListUser();
            BanCommand = new RelayCommand(Bans);
        }
        public ICommand BanCommand { get; }

        public class ListUser
        {
            public int Id { get; set; }
            public string loginuser { get; set; }
            public string email { get; set; }

            public string role { get; set; }

            public string statussub { get; set; }

            public string datareg { get; set; }

            public string lastenter { get; set; }

            public int? block { get; set; }
            
            public Visibility banbut {  get; set; }

            public Visibility unbunbut { get; set; }
            
            
        }

        public void LoadListUser()
        {
            var context = new MusicContext();
            _listUser = new ObservableCollection<ListUser>(context.Users
                .Include(u => u.Role)
                .Include(u => u.Subscription)
                .Select(u => new ListUser
                {
                    Id = u.UserId,
                    loginuser = u.UserLogin,
                    email = $"Почта: {u.Email}",
                    role = $"Роль: {u.Role.Role1}",
                    statussub = $"Подписка: {u.Subscription.Subscription1}",
                    datareg = $"Дата регистрация: {u.RegistrationDate.ToString("dd.MM.yyy")}",
                    lastenter = $"Последний вход: {u.LastLogin.ToString("dd.MM.yyy")}",
                    banbut = u.Block == 1 ? Visibility.Collapsed : Visibility.Visible,
                    unbunbut = u.Block == 1 ? Visibility.Visible : Visibility.Collapsed,
                    block = u.Block,


                })
                .OrderBy(u => u.Id)
                .ToList());
        }

        public void Bans()
        {

            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }

            var context = new MusicContext();

            var user = context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == selectedUser.Id);


            if (user == null)
                return;

            if (user.Role.RoleId == 1)
            {
                MessageBox.Show("Вы не можете заблокировать администратора", "Предупреждение");
            }
            else
            {
                if (user.Block == 1)
                {
                    var res = MessageBox.Show("Разблокировать пользователя?", "Подтверждение", MessageBoxButton.YesNo);
                    
                    if (res == MessageBoxResult.Yes)
                    {
                        user.Block = 0;
                    }
                    if (user.UserId != selectedUser.Id)
                    {
                        MessageBox.Show("Вы не можете заблокировать не выбранного пользователя", "Предупреждение");
                    }
                }
                else
                {
                    var res = MessageBox.Show("Заблокировать пользователя?", "Подтверждение", MessageBoxButton.YesNo);

                    if (res == MessageBoxResult.Yes)
                    {
                        user.Block = 1;
                    }
                }

                context.SaveChanges();
                LoadListUser();
                OnPropertyChanged(nameof(listUser));
            }


        }
    }
}
