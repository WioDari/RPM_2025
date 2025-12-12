using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spotify_wpf.Properties;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;

namespace Spotify_wpf.ViewModels
{
    class GuestViewModel :BaseViewModel
    {
        private Page _page;
        public Page page
        {
            get => _page;
            set
            {
                _page = value;
                OnPropertyChanged();
            }
        }

        public GuestViewModel()
        {
            page = new Views.Album();
            setTimer(TimeSpan.FromSeconds(30));
            _timer.Start();
        }

        private DispatcherTimer _timer;
        private TimeSpan _time = TimeSpan.FromSeconds(30);

        public void setTimer(TimeSpan time)
        {
            _time = time;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += OnTick;
        }

        private async void OnTick(object? sender, EventArgs e)
        {
            _time -= TimeSpan.FromSeconds(1);
            if (_time == TimeSpan.Zero)
            {
                _timer.Stop();
                MessageBoxResult res = MessageBox.Show("Вы хотите продолжить с регистрацией?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    Application.Current.Properties["CurrentUser"] = null;
                    Settings.Default.Reset();

                    Window auth = new Auth();
                    auth.Show();
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is not Auth)
                        {
                            w.Close();
                        }
                    }
                }
                else
                {
                    setTimer(TimeSpan.FromSeconds(30));
                    _timer.Start();
                }
            }
        }

    }
}

