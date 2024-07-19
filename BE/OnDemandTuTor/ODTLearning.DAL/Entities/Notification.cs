using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Notification
{
    public string Id { get; set; } = null!;

    public string? Description { get; set; }

    public string? IdAccount { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Status { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }
}
