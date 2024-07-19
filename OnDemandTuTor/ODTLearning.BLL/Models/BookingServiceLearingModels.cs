using ODTLearning.DAL.Entities;

namespace ODTLearning.Models
{
    public class BookingServiceLearingModels
    {

        public int? Duration { get; set; }

        public float? Price { get; set; }
        public DateOnly? date { get; set; }
        public string? timeAvalable { get; set; }
    }
    public class BookingServiceModel
    {
        public object Tutor { get; set; }
        public object User { get; set; }
        public object Service { get; set; }
        public object Booking { get; set; }
    }

}
