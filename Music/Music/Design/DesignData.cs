using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Music.Models;
using Music.Context;
using Microsoft.EntityFrameworkCore;

namespace Music.Design;

public class DesignData
{
    public List<Playlist> Playlists { get; }
    public List<Album> Albums { get; }
    public DesignData()
    {
        MusicContext context = new MusicContext();
        Playlists = context.Playlists
            .Include(x => x.Tracks)
            .Include(x => x.Tags)
            .Include(x => x.CreatorUser)
            .Include(x => x.Users)
            .ToList();

        Albums = context.Albums
            .Include(x => x.Tracks)
            .Include(x => x.Artist)
            .Include(x => x.Genres)
            .ToList();
    }
}
