using System.ComponentModel.DataAnnotations;

namespace FraudDetection.Application.DTOs
{
    public record ResendOtpDTO
    {
        [Required, EmailAddress]
        public string Email { get; init; } = " ";
    }
}
