using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Available
{
    public string Id { get; set; } = null!;

    public DateOnly? Date { get; set; }

    public string? IdTutor { get; set; }

    public string? IdTimeSlot { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual TimeSlot? IdTimeSlotNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
