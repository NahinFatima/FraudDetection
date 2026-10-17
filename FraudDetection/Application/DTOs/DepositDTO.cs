namespace FraudDetection.Application.DTOs
{
    public record DepositDTO(
        decimal Amount, 
        string IdempotencyKey);
}