using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class RequestTutor
{
    public string Id { get; set; } = null!;

    public string? Status { get; set; }

    public int? Column { get; set; }

    public string PostId { get; set; } = null!;

    public string TutorId { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;

    public virtual Tutor Tutor { get; set; } = null!;
}
