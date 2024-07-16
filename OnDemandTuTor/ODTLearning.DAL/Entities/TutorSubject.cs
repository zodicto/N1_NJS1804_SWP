using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class TutorSubject
{
    public string Id { get; set; } = null!;

    public string? IdTutor { get; set; }

    public string? IdSubject { get; set; }

    public virtual Subject? IdSubjectNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
