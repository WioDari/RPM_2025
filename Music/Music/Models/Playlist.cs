using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

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

    public virtual ObservableCollection<Tag> Tags { get; set; } = new();

    public virtual ObservableCollection<Track> Tracks { get; set; } = new();

    public virtual ObservableCollection<User> Users { get; set; } = new();

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

    public SolidColorBrush ColorName => new(CreatorUser.SubscriptionId == 2 ? Color.FromRgb(255, 148, 61) : Color.FromRgb(0, 0, 0));

    public SolidColorBrush ColorTracksCount => new(Tracks.Count == 0 ? Color.FromRgb(252, 65, 3) : Color.FromRgb(0, 0, 0));

    public DateTime? CreatedDateTime
    {
        get => DateCreated.ToDateTime(TimeOnly.MinValue);
        set => DateCreated = DateOnly.FromDateTime(value!.Value);
    }
}
