namespace FraudDetection.Application.DTOs
{
    public record LoginResultDTO
    (
        string Token,
        DateTime ExpiresAt,
        string FullName, 
        string Role
    );
}
