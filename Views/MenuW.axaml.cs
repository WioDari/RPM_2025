using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MusicPlusPlus.Context;
using MusicPlusPlus.Models;
using MusicPlusPlus.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlusPlus;

public partial class MenuW : Window
{
    public MenuW()
    {
        InitializeComponent();
        DataContext = this;
        LoadAlbums();
    }


    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        //Скрытие элементов окна согласно роли пользователя
        using (var context = new SpotifyContext())
        {
            var user = context.Users.FirstOrDefault(x => x.Userid == Settings.Default.userid);

            if (user == null)
            {
                UserNameTab.Header = "Гость";
                UsersTab.Focus();

                foreach (TabItem tab in Tabs.Items)
                {
                    tab.IsVisible = false;
                }


            }
            else
            {
                UserNameTab.Header = user.Login;

                switch (user.Roleid)
                {
                    case 3:
                        UserInfoSP.IsVisible = true; GuestInfoTBlock.IsVisible = false; break;
                    default:
                        UsersTab.IsVisible = false; UserInfoSP.IsVisible = true; GuestInfoTBlock.IsVisible = false; break;
                }

                LoginTBlock.Text = user.Login;
                FullnameTBlock.Text = user.Fullname;
            }
        }

        //старт таймера
        StartTimerLoop();
    }



    //ВЫХОД ИЗ ПРИЛОЖЕНИЯ
    private void LogoutB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Settings.Default.userid = 0;
        Settings.Default.Save();
        new MainWindow().Show();
        this.Close();
    }



    //таймер
    #region 
    private DispatcherTimer _timer;
    private TimeSpan _remainingTime = TimeSpan.FromHours(1);
    public TimeSpan _AFKLogoutTime = TimeSpan.FromMinutes(5);

    private async void StartTimerLoop()
    {
        while (_remainingTime > TimeSpan.Zero)
        {
            await Task.Delay(1000);
            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
            _AFKLogoutTime = _AFKLogoutTime.Subtract(TimeSpan.FromSeconds(1));

            if (_AFKLogoutTime <= TimeSpan.Zero)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Window w = new MessageWindow("Завершение сессии", "Сессия была завершена из-за отсутствия активности.");
                    w.Show();
                    w.Focus();
                    Settings.Default.userid = 0;
                    Settings.Default.Save();
                    new MainWindow().Show();
                    this.Close();
                });
            }

            if (_remainingTime == TimeSpan.FromMinutes(10))
            {
                new MessageWindow("Предупреждение", "Осталось 10 минут до окончания сессии.").Show();
            }

            if (_remainingTime <= TimeSpan.Zero)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Window w = new MessageWindow("Завершение сессии", "Сессия была завершена из-за истечения её срока.");
                    w.Show();
                    w.Focus();
                    Settings.Default.userid = 0;
                    Settings.Default.Save();
                    new MainWindow().Show();
                    this.Close();
                });
            }
        }
    }
    #endregion




    //ЗАГРУЗКА АЛЬБОМОВ
    public void LoadAlbums()
    {
        using (var context = new SpotifyContext())
        {
            var allalbums = context.Albums.ToList();

            AlbumsIC.Items.Clear();

            foreach (var album in allalbums)
            {
                var albumuc = new AlbumUC();
                
                albumuc.LoadAlbum(album, $"Resources/covers/{album.Albumname}.jpg");
                AlbumsIC.Items.Add(albumuc);
            }
        }
    }

    private void Grid_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        _AFKLogoutTime = TimeSpan.FromMinutes(5);
    }
}