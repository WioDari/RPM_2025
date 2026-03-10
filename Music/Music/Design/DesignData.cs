using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Linq;
using Music.Models;
using Music.Context;
using Microsoft.EntityFrameworkCore;

namespace Music.Design;

public class DesignData
{
    public List<Playlist> Playlists { get; }
    public List<Album> Albums { get; }
    public List<User> Users { get; }

    public Album TestAlbum { get; }

    public Playlist TestPlaylist { get; }


    public DesignData()
    {
        MusicContext context = new MusicContext();

        Playlists = context.Playlists
            .AsNoTracking()
            .Include(x => x.Tracks).ThenInclude(x => x.Artists)
            .Include(x => x.Tags)
            .Include(x => x.CreatorUser)
            .Include(x => x.Users)
            .ToList();

        Albums = context.Albums
            .AsNoTracking()
            .Include(x => x.Tracks).ThenInclude(x => x.Artists)
            .Include(x => x.Artist)
            .Include(x => x.Genres)
            .ToList();

        Users = context.Users.Include(x => x.Role).Include(x => x.Subscription).AsNoTracking().OrderBy(x => x.FullName).ToList();

        TestAlbum = Albums.First(x => x.Id == 39);

        TestPlaylist = Playlists.First(x => x.Id == 1);
    }
}
