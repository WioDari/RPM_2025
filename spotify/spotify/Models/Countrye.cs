using System;
using System.Collections.Generic;

namespace spotify.Models;

public partial class Countrye
{
    public int Id { get; set; }

    public string Country { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
