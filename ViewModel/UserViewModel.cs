using Microsoft.EntityFrameworkCore;
using MusicWpf.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWpf.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        private ObservableCollection<User> _users;

        public ObservableCollection<User> userspage
        {
            get => _users;

            set
            {
                _users = value;
                OnPropertyChanged();

            }
        }
         
        public UserViewModel()
        {
            LoadListUser();
        }

        public class User
        {
            public int Id { get; set; }
            public string fio { get; set; }
            public string login { get; set; }
            public string email { get; set; }

            public string role { get; set; }

            public string status { get; set; }

            public string dataregist { get; set; }

           // public string lastenter { get; set; }
        }

        public void LoadListUser()
        {
            var context = new MusicContext();
            userspage = new ObservableCollection<User>(context.Users
                .Include(u => u.Role)
                .Include(u => u.Subscription)
                .Select(u => new User
                {
                    Id = u.UserId,
                    fio = u.FullName,
                    login = $"Логин: {u.UserLogin}",
                    email = $"Почта: {u.Email}",
                    role = $"Роль: {u.Role.RoleName}",
                    status = $"Подписка: {u.Subscription.SubscriptionName}",
                    dataregist = $"Дата регистрация: {u.RegistrationDate.ToString("dd.MM.yyy")}",
                    //lastenter = $"Последний вход: {u.LastLogin.ToString("dd.MM.yyy")}",


                })
                .OrderBy(u => u.Id)
                .ToList());
        }
    }
}
