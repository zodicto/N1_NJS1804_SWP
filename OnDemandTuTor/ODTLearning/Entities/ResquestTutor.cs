using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class ResquestTutor
{
    public string IdRequestTutor { get; set; } = null!;

    public string? Status { get; set; }

    public string? IdTutor { get; set; }

    public string? IdPost { get; set; }

    public virtual Post? IdPostNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
