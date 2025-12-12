using System;
using System.Collections.Generic;

namespace Music.Models;

public partial class Playlist
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CreatorUserId { get; set; }

    public DateOnly DateCreated { get; set; }

    public int Likes { get; set; }

    public string Description { get; set; } = null!;

    public virtual User CreatorUser { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public override string ToString() => Name;

    public string Duration => GetDuration();

    private string GetDuration()
    {
        TimeSpan total = TimeSpan.Zero;
        foreach (Track t in Tracks)
        {
            total += t.Duration;
        }
        if (total < TimeSpan.FromHours(1))
            return total.ToString(@"mm\:ss");
        else
            return total.ToString(@"hh\:mm\:ss");
    }
}
