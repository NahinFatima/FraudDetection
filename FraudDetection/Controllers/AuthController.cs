using FraudDetection.Application.DTOs;
using FraudDetection.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudDetection.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController :ControllerBase
    {
        private readonly AuthService _auth;
        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _auth.RegisterAsync(dto);
            return result.Success ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDTO dto)
        {
            var result = await _auth.VerifyOtpAsync(dto);
            return result.Success ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp(ResendOtpDTO dto)
        {
            var result = await _auth.ResendOtpAsync(dto);
            return Ok(result.Value);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _auth.LoginAsync(dto);
            return result.Success ? Ok(result.Value) : Unauthorized(result.Error);
        }

    }
}
