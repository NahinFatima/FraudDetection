using FraudDetection.Application.Interfaces;
using FraudDetection.Domain.Entities;

namespace FraudDetection.Application.Rules
{
    public class AmountThresholdRule : IFraudRule
    {
        private const decimal Limit = 100_000m;
        public Task<FraudAlert?> EvaluateAsync(Transaction t)
        {
            if (t.Amount <= Limit)
            { 
                return Task.FromResult<FraudAlert?>(null);
            }

            var alert = new FraudAlert
            {
                TransactionId = t.Id,
                RuleName = "Amount Threshold",
                Reason = $"Amount {t.Amount} is above the limit of {Limit}",
                Points = 40
            };
            return Task.FromResult<FraudAlert?>(alert);

        }
    }
}
