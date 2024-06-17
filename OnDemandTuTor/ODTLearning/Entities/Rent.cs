using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Rent
{
    public string Id { get; set; } = null!;

    public string? IdSchedule { get; set; }

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Schedule? IdScheduleNavigation { get; set; }
}
