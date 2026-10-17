using System.ComponentModel.DataAnnotations;

namespace FraudDetection.Application.DTOs
{
    public record LoginDTO
    {
        [Required, EmailAddress]
        public string Email { get; init; } = "";

        [Required]
        public string Password { get; init; } = "";
    }
}
