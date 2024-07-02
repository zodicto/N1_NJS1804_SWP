namespace ODTLearning.Models
{
    public class TransactionResponse
    {
        public string Id { get; set; } = null!;

        public string? Amount { get; set; }

        public string CreateDate { get; set; }

        public string? Status { get; set; }

        public object User { get; set; }
    }
}
