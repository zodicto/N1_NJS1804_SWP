using Microsoft.EntityFrameworkCore.Migrations;

namespace ODTLearning.Models
{
    public class SignUpModelOfStudent
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? PasswordConfirm { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gmail { get; set; }

        public string? Birthdate { get; set; }

        public bool? Gender { get; set; }
    }
    public class SignUpModelOfTutor
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? PasswordConfirm { get; set; }
        public string? FirstName { get; set; }
        public string? Birthdate { get; set; }
        public string? LastName { get; set; }

        public string? Gmail { get; set; }
        public bool? Gender { get; set; }
        public string? SpecializedSkills { get; set; }
        public int? Experience { get; set; }
        public string? Organization { get; set; }
        public string? Field { get; set; }
        public string? QualificationName { get; set; }
        public string? Type { get; set; }
        public string? ImageDegree { get; set; }
    }

}
