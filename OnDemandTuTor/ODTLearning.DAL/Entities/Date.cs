using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Date
{
    public string Id { get; set; } = null!;

    public DateOnly Date1 { get; set; }

    public string? IdService { get; set; }

    public virtual Service? IdServiceNavigation { get; set; }

    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
}
