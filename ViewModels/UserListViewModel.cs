using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Spotify_wpf.Context;
using static Spotify_wpf.ViewModels.AlbumViewModel;

namespace Spotify_wpf.ViewModels
{
    class UserListViewModel : BaseViewModel
    {
        private ObservableCollection<ListUser> _listUser;

        public ObservableCollection<ListUser> listUser
        {
            get => _listUser;

            set
            {
                _listUser = value;
                OnPropertyChanged();
               
            }
        }

        public UserListViewModel()
        {
            LoadListUser();
        }

        public class ListUser
        {
            public int Id { get; set; }
            public string loginuser { get; set; }
            public string email { get; set; }

            public string role { get; set; }

            public string statussub { get; set; }

            public string datareg { get; set; }

            public string lastenter { get; set; }
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


                })
                .OrderBy(u => u.Id)
                .ToList());
        }
    }
}
