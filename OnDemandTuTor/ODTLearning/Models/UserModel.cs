namespace ODTLearning.Models
{
    public class UserModel
    {
        public string Id { get; set; } = null!;

        public int? Username { get; set; }

        public string? Password { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gmail { get; set; }

        public string? Birthdate { get; set; }

        public bool? Gender { get; set; }

        public string? Status { get; set; }
    }
}
