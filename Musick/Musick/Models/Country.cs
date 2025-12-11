using System;
using System.Collections.Generic;

namespace Musick.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string CounryName { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
