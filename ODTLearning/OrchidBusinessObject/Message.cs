using System;
using System.Collections.Generic;

namespace OrchidBusinessObject;

public partial class Message
{
    public string IdChat { get; set; } = null!;

    public string? IdAccount { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }
}
