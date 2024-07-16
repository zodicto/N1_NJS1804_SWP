namespace ODTLearning.Models
{
    public class ReviewRequestModel
    {
        public string IdUser { get; set; }
        public string FeedBack { get; set; }
        public double Rating { get; set; }
        public string IdClassRequest { get; set; }
    }

    public class ReviewServiceModel
    {
        public string IdUser { get; set; }
        public string FeedBack { get; set; }
        public double Rating { get; set; }
        public string IdBooking { get; set; }
    }
}
