using System;
using System.Collections.Generic;

namespace MusicWpf.Models;

public partial class TracksGenre
{
    public int TracksGenresId { get; set; }

    public int TracksId { get; set; }

    public int GenresId { get; set; }

    public virtual Genre Genres { get; set; } = null!;

    public virtual Track Tracks { get; set; } = null!;
}
