using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
namespace ODTLearning.Models
{
    public class RequestLearningModel
    {
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? LearningMethod { get; set; }
        public string? Class { get; set; }
        public DateOnly? Date { get; set; }
        public string? TimeStart { get; set; }
        public string? TimeEnd { get; set; }
        

    }
}