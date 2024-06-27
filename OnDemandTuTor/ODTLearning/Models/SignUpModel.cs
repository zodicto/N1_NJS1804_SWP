using Microsoft.EntityFrameworkCore.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ODTLearning.Models
{
    public class SignUpModelOfAccount
    {
        public string? fullname { get; set; }

        public string? password { get; set; }

        public string? phone { get; set; }

        public string? email { get; set; }

        public DateOnly? date_of_birth  { get; set; }

        public string ? gender { get; set; }
    }

    public class UpdateProfile
    {
        public string? fullname { get; set; }
        public string? address { get; set; }
        public string? phone { get; set; }

        public DateOnly? date_of_birth { get; set; }
        public string? avatar { get; set; }
        public string? gender { get; set; }
    }
    public class SignUpModelOfTutor 
    {
        public string? specializedSkills { get; set; }
        public int? experience { get; set; }
        public string? subject { get; set; }
        public string? qualificationame { get; set; }
        public string? type { get; set; }
        public string? introduction { get; set; }
        public IFormFile? imagequalification { get; set; }
    }

    public class SignUpModelOfTutorFB
    {
        public string? specializedskills { get; set; }
        public int? experience { get; set; }
        public string? subject { get; set; }
        public string? qualificationname { get; set; }
        public string? type { get; set; }
        public string? imagequalification { get; set; }
        public string? introduction { get; set; }
    }
    public class ListTutorToConfirmFB: SignUpModelOfTutorFB
    {
          public string? Id { get; set; }
        public string? fullname { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public string? gender { get; set; }
    }

    public class ListAccount 
    {
        public string? id { get; set; }
        public string? fullname { get; set; }

        public string? phone { get; set; }

        public string? email { get; set; }

        public DateOnly? date_of_birth { get; set; }

        public string? gender { get; set; }
        public string? roles { get; set; }
    }
}
