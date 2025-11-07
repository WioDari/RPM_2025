using System;
using System.Collections.Generic;

namespace SpotApp_wpf.Models;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public string PlaylistName { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public int Likes { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TagsInPlaylist> TagsInPlaylists { get; set; } = new List<TagsInPlaylist>();

    public virtual ICollection<TracksInPlaylist> TracksInPlaylists { get; set; } = new List<TracksInPlaylist>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UsersPlaylist> UsersPlaylists { get; set; } = new List<UsersPlaylist>();
}
