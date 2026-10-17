using FraudDetection.Application.DTOs;
using FraudDetection.Application.Interfaces;
using FraudDetection.Domain.Entities;
using FraudDetection.Application.Results;
using System.Security.Cryptography;

namespace FraudDetection.Application.Services
{
    public class AuthService
    {
        private const int OtpMins = 5;
        private const int MaxOtpAttempts = 3;
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokens;
        private readonly IOtpSender _otpSender;
        public AuthService( IAuthRepository authRepository,IPasswordHasher hasher,ITokenService tokens, IOtpSender otpSender )
        {
            _authRepository = authRepository;
            _hasher = hasher;
            _tokens = tokens;
            _otpSender = otpSender;
        }
        public async Task<Result<RegisterResultDTO>> RegisterAsync(RegisterDTO dto)
        {
            var email = Normalize(dto.Email);

            if (await _authRepository.EmailExistsAsync(email))
            {
                return Result<RegisterResultDTO>.Fail("Email is already registered.");
            }
            if (await _authRepository.CnicExistsAsync(dto.Cnic))
            {
                return Result<RegisterResultDTO>.Fail("CNIC is already registered.");
            }
            if (await _authRepository.MobileExistsAsync(dto.MobileNumber))
            {
                return Result<RegisterResultDTO>.Fail("Mobile Phone number is already registered.");
            }

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = email,
                MobileNumber = dto.MobileNumber,
                Cnic = dto.Cnic,
                PasswordHash = _hasher.Hash(dto.Password)   
            };

            user.Account = new Account
            {
                UserId = 0,
                AccountNumber = GenerateAccountNumber()
            };

            await _authRepository.AddUserAsync(user);
            await _authRepository.SaveAsync();

            await SendNewOtpAsync(user);

            return Result<RegisterResultDTO>.Ok(new RegisterResultDTO(
                user.Account.AccountNumber,
                "Registered! Enter the OTP sent to your Mobile Phone to Verify"));
        }
        public async Task<Result<string>> VerifyOtpAsync(VerifyOtpDTO dto)
        {
            var user = await _authRepository.GetUserByEmailAsync(Normalize(dto.Email));
            if (user == null || user.IsMobileVerified)
            {
                return Result<string>.Fail("Invalid request.");
            }

            var otp = await _authRepository.GetActiveOtpAsync(user.Id);
            if (otp == null)
            {
                return Result<string>.Fail("Please request an OTP to Verify first");
            }
            if (otp.ExpiresAt < DateTime.UtcNow)
            {
                return Result<string>.Fail("OTP expired, please request a new one");
            }
            if (otp.Attempts >= MaxOtpAttempts)
            { 
                return Result<string>.Fail("Too many wrong attempts, please request a new OTP"); 
            }
            if (!_hasher.Verify(dto.Code, otp.CodeHash))
            {
                otp.Attempts++;
                await _authRepository.SaveAsync();
                return Result<string>.Fail("Wrong OTP.");
            }

            otp.IsUsed = true;
            user.IsMobileVerified = true;
            await _authRepository.SaveAsync();
            return Result<string>.Ok("Mobile Phone Number Verified. You can now Log in.");
        }

        public async Task<Result<string>> ResendOtpAsync(ResendOtpDTO dto)
        {
            var user = await _authRepository.GetUserByEmailAsync(Normalize(dto.Email));
            if (user != null && !user.IsMobileVerified)
            {
                await SendNewOtpAsync(user);
            }

            return Result<string>.Ok("If the Account exists and is not Verified, a new OTP has been sent.");
        }

        public async Task<Result<LoginResultDTO>> LoginAsync(LoginDTO dto)
        {
            var user = await _authRepository.GetUserByEmailAsync(Normalize(dto.Email));

            if (user == null || !_hasher.Verify(dto.Password, user.PasswordHash))
            {
                return Result<LoginResultDTO>.Fail("Invalid Email or Password.");
            }

            if (!user.IsMobileVerified)
            {
                return Result<LoginResultDTO>.Fail("Please Verify your Mobile Phone Number first");
            }

            var (token, expiresAt) = _tokens.CreateToken(user);
            return Result<LoginResultDTO>.Ok(new LoginResultDTO(token, expiresAt, user.FullName, user.Role.ToString()));
        }

        private async Task SendNewOtpAsync(User user)
        {
            await _authRepository.InvalidateOtpsAsync(user.Id);   
            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();   
            await _authRepository.AddOtpAsync(new OtpCode
            {
                UserId = user.Id,
                CodeHash = _hasher.Hash(code),          
                ExpiresAt = DateTime.UtcNow.AddMinutes(OtpMins)
            });
            await _authRepository.SaveAsync();

            await _otpSender.SendAsync(user.MobileNumber, code);
        }
        private static string Normalize(string email) => email.Trim().ToLowerInvariant();
        private static string GenerateAccountNumber() =>
            $"PK-{RandomNumberGenerator.GetInt32(1000, 10000)}-{RandomNumberGenerator.GetInt32(100000, 1000000)}";
    }
}
