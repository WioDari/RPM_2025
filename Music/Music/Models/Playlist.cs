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

    public string Duration { get => GetDuration(); }

    private string GetDuration()
    {
        TimeSpan totalDuration = new TimeSpan();
        foreach (Track track in Tracks)
        {
            totalDuration += track.Duration;
        }
        if (totalDuration.TotalHours >= 1)
            return totalDuration.ToString(@"hh\:mm\:ss");
        else
            return totalDuration.ToString(@"mm\:ss");
    }
}
