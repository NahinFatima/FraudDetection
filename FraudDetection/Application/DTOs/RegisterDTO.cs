using System.ComponentModel.DataAnnotations;

namespace FraudDetection.Application.DTOs
{
    public record RegisterDTO
    {
        [Required, MaxLength(100)]
        public string FullName { get; init; } = " ";

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; init; } = " ";

        [Required, RegularExpression(@"^03\d{9}$", ErrorMessage = "Mobile Phone number must look like 03xxxxxxxxx")]
        public string MobileNumber { get; init; } = "";

        [Required, RegularExpression(@"^\d{5}-\d{7}-\d$", ErrorMessage = "CNIC number must look like 4xxxx-xxxxxxx-x")]
        public string Cnic { get; init; } = "";

        [Required, RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$", ErrorMessage = "Password must have 8+ characters with a letter and a number")]
        public string Password { get; init; } = "";
    }
}
