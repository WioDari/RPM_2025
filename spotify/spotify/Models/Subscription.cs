using System;
using System.Collections.Generic;

namespace spotify.Models;

public partial class Subscription
{
    public int Id { get; set; }

    public string Subscription1 { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
