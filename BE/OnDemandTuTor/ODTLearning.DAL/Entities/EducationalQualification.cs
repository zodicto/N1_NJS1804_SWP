using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class EducationalQualification
{
    public string Id { get; set; } = null!;

    public string? QualificationName { get; set; }

    public string? Img { get; set; }

    public string? Type { get; set; }

    public string? IdTutor { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
