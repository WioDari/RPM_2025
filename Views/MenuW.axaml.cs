using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using MusicPlusPlus.Context;
using MusicPlusPlus.Models;
using MusicPlusPlus.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MusicPlusPlus;

public partial class MenuW : Window
{
    public MenuW()
    {
        InitializeComponent();
        DataContext = this;
        LoadContent();
    }

    //СКРЫТИЕ ЭЛЕМЕНТОВ ОКНА СОГЛАСНО РОЛИ ПОЛЬЗОВАТЕЛЯ И СТАРТ ТАЙМЕРА
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        
        using (var context = new MusicdbContext())
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
                        AddAlbumB.IsVisible = false; UsersTab.IsVisible = false; UserInfoSP.IsVisible = true; GuestInfoTBlock.IsVisible = false; break;
                }


                //вывод информации о пользователе
                LoginTBlock.Text = user.Login;
                FullnameTBlock.Text = user.Fullname;
                SubscriptionTBlock.Text = context.Subscriptions.FirstOrDefault(x => x.Subscriptionid == user.Subscriptionid).Subscriptionname;
                
                //проверка наличия премиум-подписки и соответствующие изменения цвета элементов
                if (user.Subscriptionid == 2)
                {
                    SubscriptionTBlock.Foreground = new SolidColorBrush(Color.Parse("#ff943d"));
                    UserNameTab.Foreground = new SolidColorBrush(Color.Parse("#ff943d"));
                }
            }
        }

        //старт таймера
        StartTimerLoop();
    }



    //КНОПКИ
    #region

    //кнопка выхода
    private void LogoutB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Settings.Default.userid = 0;
        Settings.Default.Save();
        new MainWindow().Show();
        this.Close();
    }

    //открытие окна добавления альбома
    private void AddAlbumB_Click(object? sender, RoutedEventArgs e)
    {
        new AlbumCreationWindow().Show();
    }

    #endregion


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
                    new MessageWindow("Завершение сессии", "Сессия была завершена из-за отсутствия активности.").Show();
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
                    new MessageWindow("Завершение сессии", "Сессия была завершена из-за истечения её срока.").Show();
                    Settings.Default.userid = 0;
                    Settings.Default.Save();
                    new MainWindow().Show();
                    this.Close();
                });
            }
        }
    }


    //обновление таймера неактивности при движениях
    private void Grid_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        _AFKLogoutTime = TimeSpan.FromMinutes(5);
    }
    #endregion




    //ЗАГРУЗКА КОНТЕНТА
    public void LoadContent()
    {
        using (var context = new MusicdbContext())
        {
            //загрузка альбомов
            var allalbums = context.Albums.Include(x => x.Artists).ToList();
            AlbumsIC.Items.Clear();
            foreach (var album in allalbums)
            {
                var albumuc = new AlbumUC();

                albumuc.LoadAlbum(album, album.Coverpath);
                AlbumsIC.Items.Add(albumuc);
            }


            //загрузка плейлистов
            var allplaylists = context.Playlists.Include(x => x.Tracks).ToList();
            PlaylistsIC.Items.Clear();
            foreach (var playlist in allplaylists)
            {
                var playlistuc = new PlaylistUC();
                playlistuc.LoadPlaylist(playlist);
                PlaylistsIC.Items.Add(playlistuc);
            }
        }
    }


}

