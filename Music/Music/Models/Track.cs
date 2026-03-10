using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Music.Models;

public partial class Track
{
    public int Id { get; set; }

    public string TrackName { get; set; } = null!;

    public int AlbumId { get; set; }

    public TimeSpan Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int Bitrate { get; set; }

    public string FilePath { get; set; } = null!;

    public decimal Raiting { get; set; }

    public int PlayCount { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual ObservableCollection<Artist> Artists { get; set; } = new();

    public virtual ObservableCollection<Genre> Genres { get; set; } = new();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public override string ToString() => TrackName;

    public DateTime? ReleaseDateTime
    {
        get => ReleaseDate.ToDateTime(TimeOnly.MinValue);
        set => ReleaseDate = DateOnly.FromDateTime(value!.Value);
    }

    public string DurationString
    {
        get => Duration.ToString(@"hh\:mm\:ss");
        set => Duration = TimeSpan.Parse(value);
    }

    public string DurationString2
    {
        get
        {
            if (Duration.Hours > 0)
                return Duration.ToString(@"hh\:mm\:ss");
            else
                return Duration.ToString(@"mm\:ss");
        }
        set => Duration = TimeSpan.Parse(value);
    }

    public string ArtistsString => string.Join(", ", Artists.Select(x => x.Name));
}
