using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

public partial class Class
{
    public string Id { get; set; } = null!;

    public string? ClassName { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
