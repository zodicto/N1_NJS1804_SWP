using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class TimeSlot
{
    public string Id { get; set; } = null!;

    public TimeOnly? TimeSlot1 { get; set; }

    public string? Status { get; set; }

    public string? IdDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Date? IdDateNavigation { get; set; }
}
