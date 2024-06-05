using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Rent
{
    public string IdRent { get; set; } = null!;

    public string? Status { get; set; }

    public string IdStudent { get; set; } = null!;

    public string IdSchedule { get; set; } = null!;

    public virtual Schedule IdScheduleNavigation { get; set; } = null!;

    public virtual Student IdStudentNavigation { get; set; } = null!;
}
