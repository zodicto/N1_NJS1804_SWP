namespace ODTLearning.Models
{
    public class ViewRequestOfStudent
    {
        public string? IdRequest { get; set; } // New property for Account ID
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Title { get; set; }
        public string? Subject { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? LearningMethod { get; set; }
        public string? Class { get; set; }
        public DateOnly? Date { get; set; }
        public string? TimeStart { get; set; }
        public string? TimeEnd { get; set; }
    }
}
