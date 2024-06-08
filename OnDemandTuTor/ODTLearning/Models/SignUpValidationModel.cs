namespace ODTLearning.Models
{
    public class SignUpValidationOfAccountModel
    {
        public string? Username { get; set; }

        public string? Password { get; set; }


        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gmail { get; set; }
    }
    public class SignUpValidationOfTutorModel 
    {
       
        public string? SpecializedSkills { get; set; }
        public string? Organization { get; set; }
        public string? Field { get; set; }
        public string? Type { get; set; }
        public string? ImageDegree { get; set; }
        

    }
}
    