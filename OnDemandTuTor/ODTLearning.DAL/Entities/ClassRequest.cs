using ODTLearning.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class ClassRequest
{
    public string Id { get; set; } = null!;

    public string? IdTutor { get; set; }

    public string? IdRequest { get; set; }

    public virtual Request? IdRequestNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
