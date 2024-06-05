using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class ResquestLearning
{
    public string IdRequestLearning { get; set; } = null!;

    public string? Status { get; set; }

    public string IdTutor { get; set; } = null!;

    public string TidPost { get; set; } = null!;

    public virtual Tutor IdTutorNavigation { get; set; } = null!;

    public virtual Request TidPostNavigation { get; set; } = null!;
}
