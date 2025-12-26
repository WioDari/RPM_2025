using System;
using System.Collections.Generic;

namespace BelokonevPRM.Models;

public partial class Roles
{
    public int RoleId { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
