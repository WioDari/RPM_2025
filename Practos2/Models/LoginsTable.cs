using System;
using System.Collections.Generic;

namespace Practos2.Models;

public partial class LoginsTable
{
    public int Loginid { get; set; }

    public int? Userid { get; set; }

    public DateOnly? Lastenter { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
