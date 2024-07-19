using Microsoft.EntityFrameworkCore.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ODTLearning.Models
{
    public class SignUpModelOfAccount
    {
        public string? fullName { get; set; }

        public string? password { get; set; }

        public string? phone { get; set; }

        public string? email { get; set; }

        public DateOnly? date_of_birth { get; set; }

        public string? gender { get; set; }
    }

    public class UpdateProfile
    {
        public string? fullName { get; set; }
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
        public string? qualifiCationName { get; set; }
        public string? type { get; set; }
        public string? imageQualification { get; set; }
        public string? introduction { get; set; }
    }

    public class ListTutorToConfirm : SignUpModelOfTutor
    {
        public string? Id { get; set; }
        public string? fullName { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public string? gender { get; set; }
    }

    public class ListAllStudent : SignUpModelOfAccount
    {
        public string? id { get; set; }
        public string? roles { get; set; }
    }
    public class ListAllTutor
    {
        public string? Id { get; set; }
        public string? Rating { get; set; }
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? Date_of_birth { get; set; }
        public string? Gender { get; set; }
        public string? SpecializedSkills { get; set; }
        public int? Experience { get; set; }
        public string? Subjects { get; set; }
        public List<Qualification>? Qualifications { get; set; }
        public string? Introduction { get; set; }
    }

    public class Qualification
    {
        public string? IdQualifications { get; set; }
        public string? QualificationName { get; set; }
        public string? Img { get; set; }
        public string? Type { get; set; }
    }
    public class ReasonReject
    {
        public string? reason { get; set; }
    }

}
