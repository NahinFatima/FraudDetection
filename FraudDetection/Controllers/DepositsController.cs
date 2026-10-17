using FraudDetection.Application.DTOs;
using FraudDetection.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FraudDetection.Controllers
{
    [ApiController]
    [Route("api/deposits")]
    [Authorize]
    public class DepositsController : ControllerBase
    {
        private readonly DepositService _service;
        public DepositsController(DepositService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Post(DepositDTO dto)
        {
            if (dto.Amount <= 0)
            {
                return BadRequest("Deposit Amount should be greater than zero");
            }
            if(string.IsNullOrWhiteSpace(dto.IdempotencyKey))
            {
                return BadRequest("IdempotencyKey is required");
            }
            var userIdText = User.FindFirstValue(ClaimTypes.NameIdentifier);
             if (!int.TryParse(userIdText, out var userId))
            {
                return Unauthorized();
            }
            var result = await _service.DepositAsync(userId, dto);
            if(result == null)
            {
                return NotFound("Account Not Found");
            }
            return Ok(result);
        }
    }
}
