using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Booking
{
    public string Id { get; set; } = null!;

    public string? Status { get; set; }

    public int? Duration { get; set; }

    public float? Price { get; set; }

    public string? IdTimeSlot { get; set; }

    public string? IdService { get; set; }

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual TimeSlot? IdTimeSlotNavigation { get; set; }
}
