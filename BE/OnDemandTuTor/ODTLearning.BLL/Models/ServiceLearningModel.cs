namespace ODTLearning.Models
{
    public class ServiceLearningModel
    {
        public float? PricePerHour { get; set; }
        public string? Title { get; set; }
        public string? subject { get; set; }
        public string? Class { get; set; }
        public string? Description { get; set; }
        public string? LearningMethod { get; set; }
        public List<ServiceDateModel>? Schedule { get; set; }
    }

    public class ServiceDateModel
    {
        public string? Date { get; set; }
        public List<string>? TimeSlots { get; set; }
    }

}
