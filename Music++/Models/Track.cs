using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Track
{
    public int Trackid { get; set; }

    public string Trackname { get; set; } = null!;

    public string Trackduration { get; set; } = null!;

    public DateOnly Trackreleasedate { get; set; }

    public short Bitrate { get; set; }

    public string Filepath { get; set; } = null!;

    public decimal Rating { get; set; }

    public int Playcount { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
