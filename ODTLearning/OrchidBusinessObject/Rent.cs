using System;
using System.Collections.Generic;

namespace OrchidBusinessObject;

public partial class Rent
{
    public string IdRent { get; set; } = null!;

    public string? Status { get; set; }

    public string? IdAccount { get; set; }

    public string? IdSchedule { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Schedule? IdScheduleNavigation { get; set; }
}
