using System;
using System.Collections.Generic;

namespace OTDLeaningTest.Entities;

public partial class TypeOfService
{
    public string IdTypeOfService { get; set; } = null!;

    public string? NameService { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
