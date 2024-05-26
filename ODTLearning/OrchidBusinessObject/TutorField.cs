using System;
using System.Collections.Generic;

namespace OrchidBusinessObject;

public partial class TutorField
{
    public string IdTutorFileld { get; set; } = null!;

    public string? IdTutor { get; set; }

    public string? IdField { get; set; }

    public virtual Field? IdFieldNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
