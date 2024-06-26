namespace ODTLearning.Models
{
    public class UserResponse
    {

        public string Id { get; set; } = null!;

        public string IdTutor { get; set; } 

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public DateOnly? Date_of_birth { get; set; }

        public string? Gender { get; set; }

        public string? Roles { get; set; }

        public string? Avatar { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public float? AccountBalance { get; set; }
    }
}
