using MusicPlus.ViewModels;
using MusicPlus.Models;
using MusicPlus.Services;
using System;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;

namespace MusicPlus.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private User _currentUser;
        private string _currentView = "Tracks";
        private bool _isTracksActive = true;
        private bool _isAlbumsActive = false;
        private bool _isArtistsActive = false;
        private bool _isPlaylistsActive = false;
        private bool _isUsersActive = false;
        private SessionService? _sessionService;
        private DispatcherTimer? _activityTimer;

        public MenuViewModel(User user)
        {
            _currentUser = user;
            NavigateToTracksCommand = new RelayCommand(_ => { UpdateActivity(); NavigateTo("Tracks"); });
            NavigateToAlbumsCommand = new RelayCommand(_ => { UpdateActivity(); NavigateTo("Albums"); });
            NavigateToArtistsCommand = new RelayCommand(_ => { UpdateActivity(); NavigateTo("Artists"); });
            NavigateToPlaylistsCommand = new RelayCommand(_ => { UpdateActivity(); NavigateTo("Playlists"); });
            NavigateToUsersCommand = new RelayCommand(_ => { UpdateActivity(); NavigateTo("Users"); });
            LogoutCommand = new RelayCommand(_ => Logout());

            UpdateActiveStates();

            InitializeSession();
        }

        private void InitializeSession()
        {
            bool isGuest = _currentUser.Role == "Guest";
            _sessionService = new SessionService();
            _sessionService.StartSession(isGuest);

            _sessionService.SessionExpired += SessionService_SessionExpired;
            _sessionService.InactivityTimeout += SessionService_InactivityTimeout;
            _sessionService.SessionWarning += SessionService_SessionWarning;
            
            if (isGuest)
            {
                _sessionService.GuestLoginReminder += SessionService_GuestLoginReminder;
            }

            _activityTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _activityTimer.Tick += (s, e) => UpdateActivity();
            _activityTimer.Start();
        }

        private void UpdateActivity()
        {
            _sessionService?.UpdateActivity();
        }

        public string CurrentUserFullName => _currentUser.FullName;
        public string CurrentUserRole => _currentUser.Role;

        public string CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    UpdateActiveStates();
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateActiveStates()
        {
            IsTracksActive = _currentView == "Tracks";
            IsAlbumsActive = _currentView == "Albums";
            IsArtistsActive = _currentView == "Artists";
            IsPlaylistsActive = _currentView == "Playlists";
            IsUsersActive = _currentView == "Users";
        }

        public bool IsTracksActive
        {
            get => _isTracksActive;
            private set
            {
                if (_isTracksActive != value)
                {
                    _isTracksActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAlbumsActive
        {
            get => _isAlbumsActive;
            private set
            {
                if (_isAlbumsActive != value)
                {
                    _isAlbumsActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsArtistsActive
        {
            get => _isArtistsActive;
            private set
            {
                if (_isArtistsActive != value)
                {
                    _isArtistsActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsPlaylistsActive
        {
            get => _isPlaylistsActive;
            private set
            {
                if (_isPlaylistsActive != value)
                {
                    _isPlaylistsActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsUsersActive
        {
            get => _isUsersActive;
            private set
            {
                if (_isUsersActive != value)
                {
                    _isUsersActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanViewUsers => _currentUser.Role == "Admin";
        public bool CanViewArtists => _currentUser.Role != "Guest";
        public bool CanViewPlaylists => _currentUser.Role != "Guest";
        public bool CanViewAlbums => true;

        public ICommand NavigateToTracksCommand { get; }
        public ICommand NavigateToAlbumsCommand { get; }
        public ICommand NavigateToArtistsCommand { get; }
        public ICommand NavigateToPlaylistsCommand { get; }
        public ICommand NavigateToUsersCommand { get; }
        public ICommand LogoutCommand { get; }

        public event EventHandler<string>? NavigationRequested;
        public event EventHandler? LogoutRequested;
        public event EventHandler? SessionExpiredRequested;
        public event EventHandler? GuestLoginRequested;

        private void NavigateTo(string view)
        {
            CurrentView = view;
            NavigationRequested?.Invoke(this, view);
        }

        private void Logout()
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _sessionService?.StopSession();
                _activityTimer?.Stop();
                LogoutRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SessionService_SessionExpired(object? sender, EventArgs e)
        {
            _activityTimer?.Stop();
            _sessionService?.StopSession();
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Время сессии истекло. Вы будете перенаправлены на экран авторизации.", 
                    "Сессия завершена", MessageBoxButton.OK, MessageBoxImage.Information);
                SessionExpiredRequested?.Invoke(this, EventArgs.Empty);
            });
        }

        private void SessionService_InactivityTimeout(object? sender, EventArgs e)
        {
            _activityTimer?.Stop();
            _sessionService?.StopSession();
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Сессия завершена из-за неактивности (5 минут). Вы будете перенаправлены на экран авторизации.", 
                    "Неактивность", MessageBoxButton.OK, MessageBoxImage.Warning);
                SessionExpiredRequested?.Invoke(this, EventArgs.Empty);
            });
        }

        private void SessionService_SessionWarning(object? sender, EventArgs e)
        {
            var remaining = _sessionService?.GetRemainingSessionTime() ?? TimeSpan.Zero;
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                var result = MessageBox.Show(
                    $"До окончания сессии осталось {remaining.Minutes} минут {remaining.Seconds} секунд. Продолжить работу?",
                    "Предупреждение о завершении сессии",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    _activityTimer?.Stop();
                    _sessionService?.StopSession();
                    LogoutRequested?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    UpdateActivity();
                }
            });
        }

        private void SessionService_GuestLoginReminder(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var result = MessageBox.Show(
                    "Вы находитесь в гостевом режиме. Хотите войти в аккаунт для доступа к полному функционалу?",
                    "Предложение входа",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _activityTimer?.Stop();
                    _sessionService?.StopSession();
                    GuestLoginRequested?.Invoke(this, EventArgs.Empty);
                }
            });
        }
    }
}
