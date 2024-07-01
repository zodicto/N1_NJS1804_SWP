using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Booking
{
    public string Id { get; set; } = null!;

    public string? Status { get; set; }

    public string? IdAvailable { get; set; }

    public string? IdService { get; set; }

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Available? IdAvailableNavigation { get; set; }

    public virtual Service? IdServiceNavigation { get; set; }
}
