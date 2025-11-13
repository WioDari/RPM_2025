using System;
using System.Collections.Generic;

namespace Music__.Models;

public partial class Country
{
    public int Countryid { get; set; }

    public string Countryname { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
