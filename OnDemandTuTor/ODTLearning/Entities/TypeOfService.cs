using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class TypeOfService
{
    public string Id { get; set; } = null!;

    public string? NameService { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
