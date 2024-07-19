using ODTLearning.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ODTLearning.DAL.Entities;

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

    public float? AccountBalance { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual Tutor? Tutor { get; set; }
}