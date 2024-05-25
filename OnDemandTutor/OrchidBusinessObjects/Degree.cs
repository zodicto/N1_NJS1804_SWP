using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class Degree
{
    public string Id { get; set; } = null!;

    public string TutorId { get; set; } = null!;

    public int? Level { get; set; }

    public string? Specialized { get; set; }

    public virtual Tutor Tutor { get; set; } = null!;
}
