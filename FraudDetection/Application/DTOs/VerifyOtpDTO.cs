using System.ComponentModel.DataAnnotations;

namespace FraudDetection.Application.DTOs
{
    public record VerifyOtpDTO
    {
        [Required, EmailAddress]
        public string Email { get; init; } = "";

        [Required, RegularExpression(@"^\d{6}$", ErrorMessage = "Code must have 6 digits")]
        public string Code { get; init; } = "";
    }
}
