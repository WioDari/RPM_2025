using System;
using System.Collections.Generic;

namespace musicc.Models;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public string PlayName { get; set; } = null!;

    public DateOnly DateCreate { get; set; }

    public int Likes { get; set; }

    public virtual ICollection<Track> IdTracks { get; set; } = new List<Track>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
