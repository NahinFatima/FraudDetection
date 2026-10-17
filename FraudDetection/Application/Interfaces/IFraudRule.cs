using FraudDetection.Domain.Entities;

namespace FraudDetection.Application.Interfaces
{
    public interface IFraudRule
    {
        Task<FraudAlert?> EvaluateAsync(Transaction t);
    }
}
