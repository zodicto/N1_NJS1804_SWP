namespace ODTLearning.Models
{
    public class UserResponse
    {

        public string id { get; set; } = null!;

        public string? fullName { get; set; }

        public string? email { get; set; }

        public DateOnly? date_of_birth { get; set; }

        public string? gender { get; set; }

        public string? roles { get; set; }

        public string? avatar { get; set; }

        public string? address { get; set; }

        public string? phone { get; set; }

        public float? accountBalance { get; set; }
    }

    public class UserGG
    {

        public string id { get; set; } = null!;

        public string? fullName { get; set; }

        public string? email { get; set; }

        public string? roles { get; set; }

        public string? avatar { get; set; }


    }
}
