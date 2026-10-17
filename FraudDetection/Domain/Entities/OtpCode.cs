namespace FraudDetection.Domain.Entities
{
    public class OtpCode
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public User User { get; set; } = null!;
        public required string CodeHash { get; set; }
        public required DateTime ExpiresAt { get; set; }
        public int Attempts { get; set; } = 0;
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
