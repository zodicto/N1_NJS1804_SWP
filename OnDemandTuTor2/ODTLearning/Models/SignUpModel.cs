namespace ODTLearning.Models
{
    public class SignUpModel
    {
        public string Id { get; set; }
        public int? Username { get; set; }

        public string? Password { get; set; }

        public string? PasswordConfirm { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
