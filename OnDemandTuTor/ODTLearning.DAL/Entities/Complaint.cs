using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Complaint
{
    public string Id { get; set; } = null!;

    public string? Description { get; set; }

    public string? IdAccount { get; set; }

    public string? IdTutor { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Tutor? IdTutorNavigation { get; set; }
}
