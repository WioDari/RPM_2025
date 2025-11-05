using System;
using System.Collections.Generic;

namespace spotify.Models;

public partial class Playlist
{
    public int Id { get; set; }

    public string PlaylistName { get; set; } = null!;

    public int? UserId { get; set; }

    public DateOnly DateCreated { get; set; }

    public int Likes { get; set; }

    public string? Description { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
