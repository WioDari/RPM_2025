using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class Subscription
{
    public int Subid { get; set; }

    public string? Subname { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
