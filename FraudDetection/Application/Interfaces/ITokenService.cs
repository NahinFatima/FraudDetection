using FraudDetection.Domain.Entities;
namespace FraudDetection.Application.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) CreateToken(User user);
    }
}
