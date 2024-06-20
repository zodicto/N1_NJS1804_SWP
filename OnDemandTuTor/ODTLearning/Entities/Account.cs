using System;
using System.Collections.Generic;

namespace ODTLearning.Entities;

public partial class Account
{
    public string Id { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Roles { get; set; }

    public string? Avatar { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public decimal? AccountBalance { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Rent1> Rent1s { get; set; } = new List<Rent1>();

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
}
