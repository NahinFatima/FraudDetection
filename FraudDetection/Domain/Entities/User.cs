using FraudDetection.Domain.Enums;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace FraudDetection.Domain.Entities
{
    public class User
    {
        public int Id { get; set;  }
        public required string FullName {  get; set; }
        public required string Email{ get; set; }
        public required string MobileNumber { get; set; }
        public required string Cnic { get; set; } 
        public required string PasswordHash { get; set; }
        public bool IsMobileVerified { get; set; } = false;
        public VerificationStatus VerificationStatus { get; set; }
        public Role Role { get; set; } = Role.User;
        public Account? Account { get; set; }
    }
}
