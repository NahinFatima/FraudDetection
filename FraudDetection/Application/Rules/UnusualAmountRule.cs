using FraudDetection.Application.Interfaces;
using FraudDetection.Domain.Entities;

namespace FraudDetection.Application.Rules
{
    public class UnusualAmountRule : IFraudRule
    {
        private const int MinHistory = 5;
        private const decimal Multiplier = 3m;
        private readonly IFraudRepository _repository;
        public UnusualAmountRule(IFraudRepository repository)
        {
            _repository = repository;
        }
        public async Task<FraudAlert?> EvaluateAsync(Transaction t)
        {
            var count = await _repository.CountPreviousApprovedAsync(t.AccountId, t.Id); 
            if (count<MinHistory)
            { 
                return null; 
            }
            var average = await _repository.AveragePreviousApprovedAsync(t.AccountId, t.Id);
            if(t.Amount<= average *Multiplier)
            {
                return null;
            }
            return new FraudAlert
            {
                TransactionId = t.Id,
                RuleName = "UnusualAmount",
                Reason = $"Amount {t.Amount} is more than {Multiplier}x the usual average of {average:F0}",
                Points = 30
            };
        }

    }
}
