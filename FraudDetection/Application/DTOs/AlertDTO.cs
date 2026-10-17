namespace FraudDetection.Application.DTOs
{
    public record AlertDTO(
        string RuleName, 
        string Reason, 
        int Points);
}
