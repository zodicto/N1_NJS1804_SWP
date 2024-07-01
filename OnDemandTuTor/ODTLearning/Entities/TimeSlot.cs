using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class TimeSlot
{
    public string Id { get; set; } = null!;

    public TimeOnly? TimeSlot1 { get; set; }

    public virtual ICollection<Available> Availables { get; set; } = new List<Available>();
}
