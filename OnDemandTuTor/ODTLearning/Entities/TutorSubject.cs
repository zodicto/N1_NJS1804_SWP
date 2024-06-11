using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class TutorSubject
{
    public string Id { get; set; } = null!;

    public string IdTutor { get; set; } = null!;

    public string IdSubject { get; set; } = null!;

    public virtual Subject IdSubjectNavigation { get; set; } = null!;

    public virtual Tutor IdTutorNavigation { get; set; } = null!;
}
