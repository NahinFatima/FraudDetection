using FraudDetection.Domain.Entities;

namespace FraudDetection.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> CnicExistsAsync(string cnic);
        Task<bool> MobileExistsAsync(string mobileNumber);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task AddOtpAsync(OtpCode otp);
        Task<OtpCode?> GetActiveOtpAsync(int userId);
        Task InvalidateOtpsAsync(int userId);
        Task SaveAsync();
    }
}
