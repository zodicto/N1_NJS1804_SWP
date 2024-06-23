using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Rent
{
    public string Id { get; set; } = null!;

    public decimal? Price { get; set; }

    public string? IdSubject { get; set; }

    public string? IdRequest { get; set; }

    public string? IdTutor { get; set; }

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Request? IdRequestNavigation { get; set; }

    public virtual Subject? IdSubjectNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
