using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class ResquestLearning
{
    public string Id { get; set; } = null!;

    public string IdTutor { get; set; } = null!;

    public string IdRequest { get; set; } = null!;

    public virtual Request IdRequestNavigation { get; set; } = null!;

    public virtual Tutor IdTutorNavigation { get; set; } = null!;
}
