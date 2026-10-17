using FraudDetection.Application.Interfaces;

namespace FraudDetection.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string plainText) => BCrypt.Net.BCrypt.HashPassword(plainText);

        public bool Verify(string plainText, string hash)
        {
            try 
            { 
                return BCrypt.Net.BCrypt.Verify(plainText, hash); 
            }
            catch (Exception) 
            { 
                return false; 
            }  
        }
    }
}