using Microsoft.EntityFrameworkCore.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public DateOnly? date_of_birth { get; set; }
        public string? avatar { get; set; }
        public string? Gender { get; set; }
    }
    public class SignUpModelOfTutor 
    {
        public string? SpecializedSkills { get; set; }
        public int? Experience { get; set; }
        public string? Subject { get; set; }
        public string? QualificationName { get; set; }
        public string? Type { get; set; }
        public string? Introduction { get; set; }
        public IFormFile? ImageQualification { get; set; }
    }

    public class SignUpModelOfTutorFB
    {
        public string? SpecializedSkills { get; set; }
        public int? Experience { get; set; }
        public string? Subject { get; set; }
        public string? QualificationName { get; set; }
        public string? Type { get; set; }
        public string? ImageQualification { get; set; }
        public string? Introduction { get; set; }
    }
    public class ListTutorToConfirmFB: SignUpModelOfTutorFB
    {
          public string? Id { get; set; }
        public string? FullName { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public string? Gender { get; set; }
    }

    public class ListAccount 
    {
        public string? id { get; set; }
        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public DateOnly? date_of_birth { get; set; }

        public string? Gender { get; set; }
        public string? Roles { get; set; }
    }
}
