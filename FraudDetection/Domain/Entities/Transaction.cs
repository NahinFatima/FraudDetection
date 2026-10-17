using FraudDetection.Domain.Enums;
using Microsoft.Identity.Client;

namespace FraudDetection.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public required int AccountId {  get; set; }
        public Account Account { get; set; } = null!;
        public required decimal Amount { get; set; }
        public required string IdempotencyKey { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Approved;
        public int RiskScore { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<FraudAlert> Alerts { get; set; } = new();

    }
}
