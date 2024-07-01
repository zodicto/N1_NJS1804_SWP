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
        public string? Learningmethod { get; set; }
        public string? Class { get; set; }
        public string? Timestart { get; set; }
        public string? Timeend { get; set; }
        public string? Timetable { get; set; }
        public int? Totalsession { get; set; }
    }
    public class ViewRequestOfStudent : RequestLearningModel
    {
        public string? Idrequest { get; set; } // New property for Account ID
        public string? FullName { get; set; }
        public string? Status { get; set; }
    }
    public class RequestLearningResponse
    {
        public string? Idrequest { get; set; }
        public string? Title { get; set; }
        public float? Price { get; set; }
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? Learningmethod { get; set; }
        public string? Class { get; set; }
        public string? Timestart { get; set; }
        public string? Timeend { get; set; }
        public string? Timetable { get; set; }
        public int? Totalsession { get; set; }
    }
}