using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Moderator
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
