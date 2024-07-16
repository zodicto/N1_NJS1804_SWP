using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
namespace ODTLearning.Models
{
    public class RequestLearningModel
    {
        public string? Title { get; set; }
        public float? Price { get; set; }
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? LearningMethod { get; set; }
        public string? Class { get; set; }
        public string? TimeStart { get; set; }
        public string? TimeEnd { get; set; }
        public string? TimeTable { get; set; }
        public int? TotalSessions { get; set; }
    }
    public class ViewRequestOfStudent : RequestLearningModel
    {
        public string? Id { get; set; }
        public string? IdRequest { get; set; } // New property for Account ID
        public string? FullName { get; set; }
        public string? Status { get; set; }
        public string? Current { get; set; }
    }
    public class RequestLearningResponse
    {
        public string? IdRequest { get; set; }
        public string? Title { get; set; }
        public float? Price { get; set; }
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? LearningMethod { get; set; }
        public string? Class { get; set; }
        public string? TimeStart { get; set; }
        public string? TimeEnd { get; set; }
        public string? TimeTable { get; set; }
        public string? Status {get; set; }
        public int? TotalSessions { get; set; }
    }
}