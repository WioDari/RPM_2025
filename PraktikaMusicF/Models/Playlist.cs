using System;
using System.Collections.Generic;

namespace PraktikaMusicF.Models;

public partial class Playlist
{
    public int PlId { get; set; }

    public string? PlName { get; set; }

    public int? PlTracksQuantity { get; set; }

    public DateOnly? PlDateCreate { get; set; }

    public int? PlLikes { get; set; }

    public string? PlDesc { get; set; }

    public TimeOnly? TotalDuration { get; set; }

    public virtual ICollection<PlaylistCreator> PlaylistCreators { get; set; } = new List<PlaylistCreator>();

    public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();

    public virtual ICollection<PlaylistsTag> PlaylistsTags { get; set; } = new List<PlaylistsTag>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
