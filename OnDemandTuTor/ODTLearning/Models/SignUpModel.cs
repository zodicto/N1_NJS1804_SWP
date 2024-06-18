using Microsoft.EntityFrameworkCore.Migrations;

namespace ODTLearning.Models
{
    public class SignUpModelOfAccount
    {
        public string? FullName { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public DateOnly? date_of_birth  { get; set; }

        public string ? Gender { get; set; }
    }

    public class UpdateProfile
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public string? Email { get; set; }

        public DateOnly? date_of_birth { get; set; }

        public string? Gender { get; set; }
    }
    public class SignUpModelOfTutor 
    {
        public string? SpecializedSkills { get; set; }
        public int? Experience { get; set; }
        public string? Subject { get; set; }
        public string? QualificationName { get; set; }
        public string? Type { get; set; }
        public IFormFile? ImageQualification { get; set; }
    }

}
