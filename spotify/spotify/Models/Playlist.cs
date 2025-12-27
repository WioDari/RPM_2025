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

    public string PlaylistNameAndFullName
    {
        get
        {
            if (User != null)
            {
                return $"{PlaylistName} | {User.FullName}";
            }
            else
            {
                return PlaylistName;
            }
        }
    }

    public string Duration
    {
        get
        {
            TimeSpan total = TimeSpan.Zero;
            foreach (var track in Tracks)
            {
                total += track.Duration;
            }
            return total.ToString(@"hh\:mm\:ss");
        }
    }
}
