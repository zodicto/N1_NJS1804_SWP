namespace ODTLearning.Models
{
    public class ViewRequestOfStudent
    {
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? LearningMethod{ get; set; }
        public string? LearningModel { get; set; }
        public DateOnly? Date { get; set; }
        public string? Time { get; set; }
    }
}
