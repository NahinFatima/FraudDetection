using FraudDetection.Application.Interfaces;
using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FraudDetection.Infrastructure.Repositories
{
    public class FraudRepository : IFraudRepository
    {
        private readonly FraudDetectionDbContext _db;
        public FraudRepository( FraudDetectionDbContext db)
        {
            _db = db;
        }
        public Task<Account?> GetAccountByUserIdAsync(int userId) =>
            _db.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
        public Task<Transaction?> GetTransactionByKeyAsync(int accountId, string idempotencyKey) =>
            _db.Transactions.Include(t => t.Alerts).Include(t => t.Account).FirstOrDefaultAsync(t => t.AccountId == accountId && t.IdempotencyKey == idempotencyKey);
        public async Task AddTransactionAsync(Transaction t) => await _db.Transactions.AddAsync(t);
        public async Task AddAlertsAsync(IEnumerable<FraudAlert> alerts) => await _db.FraudAlerts.AddRangeAsync(alerts);
        public Task<int>CountRecentTransactionsAsync(int accountId, DateTime since) =>
            _db.Transactions.CountAsync(t => t.AccountId == accountId && t.CreatedAt >= since);
        public Task<int> CountPreviousApprovedAsync(int accountId, int excludeTransactionId) =>
            _db.Transactions.CountAsync(t => t.AccountId == accountId && t.Id != excludeTransactionId && t.Status == TransactionStatus.Approved);
        public Task<decimal> AveragePreviousApprovedAsync(int accountId, int excludeTransactionId) =>
            _db.Transactions.Where(t => t.AccountId == accountId && t.Id != excludeTransactionId && t.Status == TransactionStatus.Approved).AverageAsync(t => t.Amount);
        public Task SaveAsync() => _db.SaveChangesAsync();
    }
}
