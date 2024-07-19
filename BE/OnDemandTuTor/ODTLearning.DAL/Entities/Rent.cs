using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Rent
{
    public string Id { get; set; } = null!;

    public float? Price { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? IdTutor { get; set; }

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
