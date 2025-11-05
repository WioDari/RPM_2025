using System;
using System.Collections.Generic;

namespace Spotify_wpf.Models;

public partial class Playlist
{
    public int PlayListId { get; set; }

    public string PlaylistName { get; set; } = null!;

    public DateOnly DataCreate { get; set; }

    public int Likes { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<PlayListTrack> PlayListTracks { get; set; } = new List<PlayListTrack>();

    public virtual ICollection<PlaylistUser> PlaylistUsers { get; set; } = new List<PlaylistUser>();

    public virtual ICollection<TagsPlaylist> TagsPlaylists { get; set; } = new List<TagsPlaylist>();

    public virtual User User { get; set; } = null!;
}
