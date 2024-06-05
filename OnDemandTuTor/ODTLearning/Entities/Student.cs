using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Student
{
    public string IdStudent { get; set; } = null!;

    public string? Img { get; set; }

    public string IdAccount { get; set; } = null!;

    public virtual Acount IdAccountNavigation { get; set; } = null!;

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
