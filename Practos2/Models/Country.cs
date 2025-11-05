using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Country
{
    public int Countryid { get; set; }

    public string? Countryname { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
