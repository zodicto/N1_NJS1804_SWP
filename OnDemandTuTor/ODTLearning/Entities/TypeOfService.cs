using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class TypeOfService
{
    public string IdTypeOfService { get; set; } = null!;

    public int? NameService { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
