using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Review
{
    public string Id { get; set; } = null!;

    public string? Feedback { get; set; }

    public double? Rating { get; set; }

    public string? IdAccount { get; set; }

    public string? IdTutor { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
