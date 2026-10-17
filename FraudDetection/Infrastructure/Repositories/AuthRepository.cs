using FraudDetection.Application.Interfaces;
using FraudDetection.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FraudDetection.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly FraudDetectionDbContext _db;
        public AuthRepository(FraudDetectionDbContext db)
        {
            _db = db;
        }
        public Task<bool> EmailExistsAsync(string email) => _db.Users.AnyAsync(u => u.Email == email);
        public Task<bool> CnicExistsAsync(string cnic) => _db.Users.AnyAsync(u => u.Cnic == cnic);
        public Task<bool> MobileExistsAsync(string mobileNumber) => _db.Users.AnyAsync(u => u.MobileNumber == mobileNumber);
        public Task<User?> GetUserByEmailAsync(string email) =>  _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        public async Task AddUserAsync(User user) =>  await _db.Users.AddAsync(user);
        public async Task AddOtpAsync(OtpCode otp) =>  await _db.OtpCodes.AddAsync(otp);
        public Task<OtpCode?> GetActiveOtpAsync(int userId) =>_db.OtpCodes.Where(o => o.UserId == userId && !o.IsUsed).OrderByDescending(o => o.CreatedAt).FirstOrDefaultAsync();
        public Task InvalidateOtpsAsync(int userId) => _db.OtpCodes.Where(o => o.UserId == userId && !o.IsUsed).ExecuteUpdateAsync(s => s.SetProperty(o => o.IsUsed, true));
        public Task SaveAsync() => _db.SaveChangesAsync();
    }
}
