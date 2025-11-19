using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotApp_wpf.ViewModels
{
    public class UserListViewModel
    {
        public UserListViewModel() 
        {
            LoadUsers();
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
        }

        public ObservableCollection<UserTmp> userTmps { get; set; }

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
                   role = u.Role.RoleName,
                   subStatus = u.Subscription.SubscriptionTittle,
                   regDate = u.RegistrationDate.ToString("dd.MM.yyy"),
                   lastEntranceDate = u.LastLogin.ToString("dd.MM.yyy")
                })
                .ToList());
        }
    }
}
