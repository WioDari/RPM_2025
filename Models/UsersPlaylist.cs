using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class UsersPlaylist
{
    public int UserPlaylistId { get; set; }

    public int PlaylistId { get; set; }

    public int UserId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
