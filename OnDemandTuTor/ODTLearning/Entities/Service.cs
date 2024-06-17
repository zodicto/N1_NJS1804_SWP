using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Service
{
    public string Id { get; set; } = null!;

    public float? Price { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Video { get; set; }

    public string? IdTutor { get; set; }

    public string? IdClass { get; set; }

    public virtual Class? IdClassNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
