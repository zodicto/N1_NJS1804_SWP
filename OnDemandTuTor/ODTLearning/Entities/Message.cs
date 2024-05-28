using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Message
{
    public string IdChat { get; set; } = null!;

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }
}
