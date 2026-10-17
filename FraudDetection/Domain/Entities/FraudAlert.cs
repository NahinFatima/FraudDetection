namespace FraudDetection.Domain.Entities
{
    public class FraudAlert
    {
        public int Id { get; set; }
        public required int TransactionId { get; set; } 
        public Transaction Transaction { get; set; } = null!;
        public required string RuleName { get; set; }
        public required string Reason { get; set; }
        public required int Points { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
