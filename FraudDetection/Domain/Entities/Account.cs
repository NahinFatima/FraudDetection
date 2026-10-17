namespace FraudDetection.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public User User { get; set; } = null!;
        public required string AccountNumber { get; set; }
        public decimal Balance { get; set; } = 0;
        public List<Transaction> Transaction { get; set; } = new(); 
    }
}
