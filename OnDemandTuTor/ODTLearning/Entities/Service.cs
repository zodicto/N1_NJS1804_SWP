using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Service
{
    public string Id { get; set; } = null!;

    public float? PricePerHour { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? IdTutor { get; set; }

    public string? IdClass { get; set; }

    public string? IdSubject { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Class? IdClassNavigation { get; set; }

    public virtual Subject? IdSubjectNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
