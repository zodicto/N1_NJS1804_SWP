using Microsoft.EntityFrameworkCore.Migrations;

namespace ODTLearning.Models
{
    public class SignUpModelOfAccount
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? phone { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public DateOnly? date_of_birth  { get; set; }

        public string ? Gender { get; set; }
    }
    public class SignUpModelOfTutor 
    {
        public string? SpecializedSkills { get; set; }
        public int? Experience { get; set; }
        public string? Field { get; set; }
        public string? QualificationName { get; set; }
        public string? Type { get; set; }
        public string? ImageDegree { get; set; }
    }

}
