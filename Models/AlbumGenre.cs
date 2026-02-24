using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class AlbumGenre
{
    public int? Album { get; set; }

    public int? Agenre { get; set; }

    public virtual Genre? AgenreNavigation { get; set; }

    public virtual Album? AlbumNavigation { get; set; }
}
