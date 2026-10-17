
using FraudDetection.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace FraudDetection.Infrastructure.Security
{
    public class ConsoleOtpSender : IOtpSender
    {
        public Task SendAsync(string mobileNumber, string code)
        {
            Console.WriteLine($"[OTP] Code for {mobileNumber}: {code}");
            return Task.CompletedTask;
        }
    }
}
