using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class TypeOfService
{
    public string Id { get; set; } = null!;

    public int? Name { get; set; }

    public string PostId { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
