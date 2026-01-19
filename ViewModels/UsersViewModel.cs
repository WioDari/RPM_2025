using MusicPlus.ViewModels;
using MusicPlus.Models;
using MusicPlus.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MusicPlus.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        private readonly ApplicationDbContext _context;
        private ObservableCollection<UserViewModel> _users;

        public UsersViewModel()
        {
            var connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=12345;Port=3306;CharSet=utf8mb4;";
            var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;
            
            _context = new ApplicationDbContext(options);

            _users = new ObservableCollection<UserViewModel>();

            LoadUsersCommand = new RelayCommand(_ => LoadUsers());
            BlockUnblockUserCommand = new RelayCommand(user => BlockUnblockUser(user as UserViewModel));

            LoadUsers();
        }

        public ObservableCollection<UserViewModel> Users => _users;

        public ICommand LoadUsersCommand { get; }
        public ICommand BlockUnblockUserCommand { get; }

        public event EventHandler? RefreshRequested;

        private void LoadUsers()
        {
            try
            {
                _users.Clear();

                var users = _context.Users
                    .OrderBy(u => u.UserLogin)
                    .ToList();

                foreach (var user in users)
                {
                    var userVm = new UserViewModel
                    {
                        UserID = user.UserID,
                        UserLogin = user.UserLogin,
                        Email = user.Email,
                        Role = user.Role,
                        Subscription = user.Subscription ?? "Без подписки",
                        RegistrationDate = user.RegistrationDate,
                        LastLogin = user.LastLogin,
                        IsBlocked = user.IsBlocked,
                        CanBlock = user.Role != "Admin"
                    };

                    _users.Add(userVm);
                }

                OnPropertyChanged(nameof(Users));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void BlockUnblockUser(UserViewModel? user)
        {
            if (user == null || !user.CanBlock) return;

            var action = user.IsBlocked ? "разблокировать" : "заблокировать";
            var result = System.Windows.MessageBox.Show(
                $"Вы уверены, что хотите {action} пользователя \"{user.UserLogin}\"?",
                $"Подтверждение {action}",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    var userToUpdate = _context.Users.Find(user.UserID);
                    if (userToUpdate != null)
                    {
                        userToUpdate.IsBlocked = !userToUpdate.IsBlocked;
                        _context.SaveChanges();
                        LoadUsers();
                        RefreshRequested?.Invoke(this, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка обновления пользователя: {ex.Message}", "Ошибка", 
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }
    }

    public class UserViewModel : BaseViewModel
    {
        private int _userID;
        private string _userLogin = "";
        private string _email = "";
        private string _role = "";
        private string _subscription = "";
        private DateTime _registrationDate;
        private DateTime? _lastLogin;
        private bool _isBlocked;
        private bool _canBlock;

        public int UserID
        {
            get => _userID;
            set { _userID = value; OnPropertyChanged(); }
        }

        public string UserLogin
        {
            get => _userLogin;
            set { _userLogin = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(); }
        }

        public string Subscription
        {
            get => _subscription;
            set { _subscription = value; OnPropertyChanged(); }
        }

        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set { _registrationDate = value; OnPropertyChanged(); }
        }

        public DateTime? LastLogin
        {
            get => _lastLogin;
            set { _lastLogin = value; OnPropertyChanged(); }
        }

        public bool IsBlocked
        {
            get => _isBlocked;
            set { _isBlocked = value; OnPropertyChanged(); OnPropertyChanged(nameof(BlockStatus)); }
        }

        public bool CanBlock
        {
            get => _canBlock;
            set { _canBlock = value; OnPropertyChanged(); }
        }

        public string BlockStatus => _isBlocked ? "Заблокирован" : "Активен";
    }
}
