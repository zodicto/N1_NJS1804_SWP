using System;
namespace ODTLearning.Models
{
    public class RequestLearningModel
    {
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? NameService { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly? TimeStart { get; set; }
        public TimeOnly? TimeEnd { get; set; }

    }
}