using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static SpotApp_wpf.ViewModels.AlbumViewModel;

namespace SpotApp_wpf.ViewModels
{
    public class UserListViewModel : BaseViewModel
    {
        public ICommand BanCommand { get;  }
        public UserListViewModel() 
        {
            LoadUsers();
            BanCommand = new RelayCommand(BanF);
        }

        public class UserTmp
        {
            public string id { get; set; }
            public string login { get; set; }
            public string email { get; set; }
            public string role { get; set; }
            public string subStatus { get; set; }
            public string regDate { get; set; }
            public string lastEntranceDate { get; set; }
            public Visibility banVis { get; set; }
            public Visibility unBanVis { get; set; }
        }

        public ObservableCollection<UserTmp> userTmps { get; set; }

        /*private bool _btnVisibility {  get; set; }
        public bool btnVisibility
        {
            get => _btnVisibility;
            set
            {
                _btnVisibility = value;
                OnPropertyChanged();
            }
        }*/
        public UserTmp selectedUser { get; set; }

        public void LoadUsers()
        {
            var context = new SpotifyContext();
            userTmps = new ObservableCollection<UserTmp>(context.Users
                .Include(u => u.Role)
                .Include(u => u.Subscription)
                .Select(u => new UserTmp
                {
                    id = u.UserId.ToString(),
                    login = u.UserLogin,
                    email = u.Email,
                    role = u.Role.RoleName == "User" ? "Пользователь" : u.Role.RoleName == "Manager" ? "Менеджер" : u.Role.RoleName == "Guest" ? "Гость" : u.Role.RoleName == "Admin" ? "Админ" : "Ошибка",
                    subStatus = u.Subscription.SubscriptionTittle,
                    regDate = u.RegistrationDate.ToString("dd.MM.yyy"),
                    lastEntranceDate = u.LastLogin.ToString("dd.MM.yyy"),
                    banVis = u.RoleId == 4? Visibility.Hidden: u.Ban == 1 ? Visibility.Collapsed : Visibility.Visible,
                    unBanVis = u.RoleId == 4 ? Visibility.Hidden: u.Ban == 1 ? Visibility.Visible : Visibility.Collapsed,
                })
                .ToList());
        }
        public void BanF()
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var context = new SpotifyContext();
            var user = context.Users.FirstOrDefault(u => u.UserId == Convert.ToInt32(selectedUser.id));
            if (user.RoleId != 4 && user.Ban == 1)
            {
                user.Ban = 0;
                LoadUsers();
                MessageBox.Show($"Пользователь {user.UserLogin} разбанен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (user.RoleId != 4 && user.Ban == 0)
            {
                user.Ban = 1;
                LoadUsers();
                MessageBox.Show($"Пользователь {user.UserLogin} забанен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

                context.SaveChanges();
            LoadUsers();
            OnPropertyChanged(nameof(userTmps));
        }
    }
}
