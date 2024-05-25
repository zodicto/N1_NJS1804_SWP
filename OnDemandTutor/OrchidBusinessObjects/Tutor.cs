using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class Tutor
{
    public string Id { get; set; } = null!;

    public string? SpecializedSkills { get; set; }

    public int? Experience { get; set; }

    public string UserId { get; set; } = null!;

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual ICollection<Degree> Degrees { get; set; } = new List<Degree>();

    public virtual ICollection<RequestTutor> RequestTutors { get; set; } = new List<RequestTutor>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual User User { get; set; } = null!;
}
