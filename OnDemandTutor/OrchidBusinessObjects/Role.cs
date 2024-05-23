using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string? NameRole { get; set; }

    public int? Description { get; set; }

    public int? Column { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
