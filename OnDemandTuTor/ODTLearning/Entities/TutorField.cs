using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class TutorField
{
    public string Id { get; set; } = null!;

    public string IdTutor { get; set; } = null!;

    public string IdField { get; set; } = null!;

    public virtual Field IdFieldNavigation { get; set; } = null!;

    public virtual Tutor IdTutorNavigation { get; set; } = null!;
}
