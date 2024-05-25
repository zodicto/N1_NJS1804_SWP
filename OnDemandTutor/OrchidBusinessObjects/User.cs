using System;
using System.Collections.Generic;

namespace OrchidBusinessObjects;

public partial class User
{
    public string Id { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gmail { get; set; }

    public string? Birthdate { get; set; }

    public bool? Gender { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Moderator> Moderators { get; set; } = new List<Moderator>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
