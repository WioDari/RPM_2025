using System;
using System.Collections.Generic;

namespace PracticeWork.Models;

public partial class ArtistGenre
{
    public int? Artist { get; set; }

    public int? Agenre { get; set; }

    public virtual Genre? AgenreNavigation { get; set; }

    public virtual Artist? ArtistNavigation { get; set; }
}
