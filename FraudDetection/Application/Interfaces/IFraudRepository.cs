using FraudDetection.Domain.Entities;

namespace FraudDetection.Application.Interfaces
{
    public interface IFraudRepository
    {
        Task<Account?> GetAccountByUserIdAsync(int userId);
        Task<Transaction?> GetTransactionByKeyAsync(int accountId, string IdempotencyKey);
        Task AddTransactionAsync(Transaction t);
        Task AddAlertsAsync(IEnumerable<FraudAlert> alerts);
        Task<int> CountRecentTransactionsAsync(int accountId, DateTime since);
        Task<int> CountPreviousApprovedAsync(int accountId, int excludeTransactionId);
        Task<decimal> AveragePreviousApprovedAsync(int accountId, int excludeTransactionId);
        Task SaveAsync();


    }
}
