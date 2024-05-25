using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class Service
{
    public string Id { get; set; } = null!;

    public float? Price { get; set; }

    public string? Status { get; set; }

    public string TypeOfServiceId { get; set; } = null!;

    public string TutorId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual Tutor Tutor { get; set; } = null!;

    public virtual TypeOfService TypeOfService { get; set; } = null!;
}
