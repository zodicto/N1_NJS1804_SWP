using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Rent
{
    public string Id { get; set; } = null!;

    public string IdSchedule { get; set; } = null!;

    public string IdAccount { get; set; } = null!;

    public virtual Account IdAccountNavigation { get; set; } = null!;

    public virtual Schedule IdScheduleNavigation { get; set; } = null!;
}
