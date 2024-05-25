using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class Rent
{
    public string Id { get; set; } = null!;

    public string? Status { get; set; }

    public string ServiceId { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
