using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Booking
{
    public string Id { get; set; } = null!;

    public double? Duration { get; set; }

    public double? Price { get; set; }

    public string? Status { get; set; }

    public string? IdTimeSlot { get; set; }

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual TimeSlot? IdTimeSlotNavigation { get; set; }
}
