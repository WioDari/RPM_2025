using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using musicc.Context;
using musicc.Models;

namespace musicc;

public partial class Window1 : Window
{
    public string Boxname { get; set; } = "none";
    public string Premium { get; set; } = "none";
    public Playlist CurrentPlaylist { get; set; }
    public Window1()
    {
       
        InitializeComponent();

        
    }

    public Window1(string boxname, bool? premium)
    {
        Boxname = boxname;
        Premium = premium?.ToString() ?? "none";
        InitializeComponent();

        LoadPlay();
    }

    private async void LoadPlay()
    {
        CurrentPlaylist = await Playlist.CreatePl(Boxname);
        DataContext = this;
    }
    public class Playlist
    {
        public string Name { get; set; }
        public string author { get; set; }
        public  int like { get; set; }
        public int track { get; set; }
        public System.DateOnly data { get; set; }

        public static async Task<Playlist> CreatePl(string Boxname)
        {
            var playlist = new Playlist();
            await playlist.NEWe(Boxname);
            return playlist;
        }
        public async Task NEWe(string Boxname)
        {
            using ( var bd = new PostgresContext())
            {
                var viewPlay = await bd.Users.Include(u => u.Playlists).FirstOrDefaultAsync(u => u.FullName == Boxname);
                if (viewPlay != null && viewPlay.Playlists != null && viewPlay.Playlists.Any())
                {
                    var firstPlaylist = viewPlay.Playlists.First();
                
                    Name = firstPlaylist.PlayName;
                    author = firstPlaylist.Users.First().FullName;
                    like = firstPlaylist.Likes;
                    data = firstPlaylist.DateCreate;
                }
            }

        }
    }
}