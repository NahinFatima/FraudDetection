namespace FraudDetection.Application.Interfaces
{
    public interface IOtpSender
    {
        Task SendAsync(string mobileNumber, string code);
    }
}
