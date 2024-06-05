using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class EducationalQualification
{
    public string IdEducationalEualifications { get; set; } = null!;

    public string? CertificateName { get; set; }

    public string? Img { get; set; }

    public string? Type { get; set; }

    public string IdTutor { get; set; } = null!;

    public virtual Tutor IdTutorNavigation { get; set; } = null!;
}
