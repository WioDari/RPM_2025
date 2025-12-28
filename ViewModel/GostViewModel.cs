using MusicWpf.Properties;
using MusicWpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MusicWpf.ViewModel
{
    internal class GostViewModel : BaseViewModel
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

        public GostViewModel()
        {
            page = new AlbumPage();
            setTimer(TimeSpan.FromSeconds(21));
            _timer.Start();
        }

        private DispatcherTimer _timer;
        private TimeSpan _time = TimeSpan.FromSeconds(21);

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
                MessageBoxResult res = MessageBox.Show("Вы хотите войти в аккаунт?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    Application.Current.Properties["CurrentUser"] = null;
                    Settings.Default.Reset();

                    Window auth = new Views.Auth();
                    auth.Show();
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is not Views.Auth)
                        {
                            w.Close();
                        }
                    }
                }
                else
                {
                    setTimer(TimeSpan.FromSeconds(21));
                    _timer.Start();
                }
            }

        }
    }
}
