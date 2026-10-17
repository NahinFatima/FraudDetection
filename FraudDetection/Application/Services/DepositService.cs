using FraudDetection.Application.Interfaces;
using FraudDetection.Application.DTOs;
using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Enums;

namespace FraudDetection.Application.Services
{
    public class DepositService
    {
        private readonly IFraudRepository _repository;
        private readonly IEnumerable<IFraudRule> _rules;
        public DepositService(IFraudRepository repository,IEnumerable<IFraudRule> rules)
        {
            _repository = repository;
            _rules = rules;
        }
        public async Task<DepositResultDTO?> DepositAsync(int userId, DepositDTO dto)
        {
            var account = await _repository.GetAccountByUserIdAsync(userId);
            if (account == null)
            {
                return null;
            }
            var existing = await _repository.GetTransactionByKeyAsync(account.Id, dto.IdempotencyKey);
            if(existing != null)
            {
                var oldAlerts = existing.Alerts.Select(a => new AlertDTO(a.RuleName, a.Reason, a.Points)).ToList();
                return BuildResult(existing, oldAlerts,existing.Account.Balance);
            }

            var t = new Transaction
            {
                AccountId = account.Id,
                Amount = dto.Amount,
                IdempotencyKey = dto.IdempotencyKey
            };
            await _repository.AddTransactionAsync(t);
            await _repository.SaveAsync();
            var alerts = new List<FraudAlert>();
            foreach(var rule in _rules)
            {
                var alert = await rule.EvaluateAsync(t);
                if (alert != null)
                {
                    alerts.Add(alert);
                }
            }
            t.RiskScore = alerts.Sum(a => a.Points);
            if(alerts.Count >0)
            {
                t.Status = TransactionStatus.UnderReview;
            }
            else 
            { 
                account.Balance = account.Balance + t.Amount; 
            }
            await _repository.AddAlertsAsync(alerts);
            await _repository.SaveAsync();
            var alertDtos = alerts.Select(a => new AlertDTO(a.RuleName, a.Reason, a.Points)).ToList();
            return BuildResult(t, alertDtos, account.Balance);
        }
        private static DepositResultDTO BuildResult(Transaction tx, List<AlertDTO> alerts, decimal balance)
        {
            var level = tx.RiskScore >= 60 ? "High" : tx.RiskScore >= 30 ? "Medium" : "Low";
            return new DepositResultDTO(tx.Id, tx.Amount, tx.Status.ToString(), tx.RiskScore, level, balance, alerts);
        }
    }
}
