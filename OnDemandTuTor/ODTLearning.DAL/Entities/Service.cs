using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Service
{
    public string Id { get; set; } = null!;

    public float? PricePerHour { get; set; }

    public string? Title { get; set; }

    public string? LearningMethod { get; set; }

    public string? Description { get; set; }

    public string? IdTutor { get; set; }

    public string? IdClass { get; set; }

    public string? IdSubject { get; set; }

    public virtual ICollection<Date> Dates { get; set; } = new List<Date>();

    public virtual Class? IdClassNavigation { get; set; }

    public virtual Subject? IdSubjectNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
