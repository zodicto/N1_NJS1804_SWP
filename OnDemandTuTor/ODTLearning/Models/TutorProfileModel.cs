namespace ODTLearning.Models
{
    public class TutorProfileModel
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gmail { get; set; }

        public DateOnly? Birthdate { get; set; }

        public string? Gender { get; set; }
        public object Fields { get; set; }
        public object Qualifications { get; set; }
    }
}
