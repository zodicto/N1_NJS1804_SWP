using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual User User { get; set; } = null!;
}
