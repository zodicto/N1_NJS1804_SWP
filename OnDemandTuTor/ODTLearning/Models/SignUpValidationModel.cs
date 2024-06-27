namespace ODTLearning.Models
{
    public class SignUpValidationOfAccountModel
    {
        public string? Username { get; set; }

        public string? Password { get; set; }


        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? Gmail { get; set; }
    }
    public class SignUpValidationOfTutorModel 
    {
       
        public string? Specializedskills { get; set; }
        public string? Organization { get; set; }
        public string? Field { get; set; }
        public string? Type { get; set; }
        public string? Imagedegree { get; set; }
        

    }
}
    