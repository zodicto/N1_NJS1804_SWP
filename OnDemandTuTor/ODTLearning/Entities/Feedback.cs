using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Feedback
{
    public string IdFeedback { get; set; } = null!;

    public string? Description { get; set; }

    public string? IdAccount { get; set; }

    public string? IdService { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Service? Service { get; set; }
}
