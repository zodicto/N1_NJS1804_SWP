using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class Certificate
{
    public string Id { get; set; } = null!;

    public string TutorId { get; set; } = null!;

    public string? CertificateName { get; set; }

    public string? Organization { get; set; }

    public virtual Tutor Tutor { get; set; } = null!;
}
