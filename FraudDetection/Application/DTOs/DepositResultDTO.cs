namespace FraudDetection.Application.DTOs
{
    public record DepositResultDTO(
        int TransactionId,
        decimal Amount,
        string Status,
        int RiskScore,
        string RiskLevel,
        decimal Balance,
        List<AlertDTO> Alerts);
}
