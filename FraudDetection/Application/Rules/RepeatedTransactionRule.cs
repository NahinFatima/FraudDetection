using FraudDetection.Application.Interfaces;
using FraudDetection.Domain.Entities;

namespace FraudDetection.Application.Rules
{
    public class RepeatedTransactionRule: IFraudRule
    {
        private readonly IFraudRepository _repository;
        public RepeatedTransactionRule(IFraudRepository repository)
        {
            _repository = repository;
        }
        public async Task<FraudAlert?> EvaluateAsync(Transaction t)
        {
            var since = t.CreatedAt.AddMinutes(-5);
            var count = await _repository.CountRecentTransactionsAsync(t.AccountId, since);
            if (count < 3)
            {
                return null;
            }
            return new FraudAlert
            {
                TransactionId = t.Id,
                RuleName = "RepeatedTransaction",
                Reason = $"{count} transactions within 5 minutes",
                Points = 30
            };

        }
    }
}
