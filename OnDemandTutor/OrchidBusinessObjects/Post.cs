using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class Post
{
    public string Id { get; set; } = null!;

    public string? Price { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string StudentId { get; set; } = null!;

    public virtual ICollection<RequestTutor> RequestTutors { get; set; } = new List<RequestTutor>();

    public virtual Student Student { get; set; } = null!;

    public virtual ICollection<TypeOfService> TypeOfServices { get; set; } = new List<TypeOfService>();
}
