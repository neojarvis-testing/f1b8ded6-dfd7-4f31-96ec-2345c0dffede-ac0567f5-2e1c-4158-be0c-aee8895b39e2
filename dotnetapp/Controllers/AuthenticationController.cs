using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        // ----- Login Endpoint -----
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid login request." });

            var (statusCode, responseMessage) = await _authService.Login(model);
            if (statusCode == 1)
                return Ok(new { token = responseMessage });

            return Unauthorized(new { Message = responseMessage });
        }

        // ----- Registration Endpoint -----
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid registration request." });

            var (statusCode, responseMessage) = await _authService.Register(model);
            if (statusCode == 1)
                return Ok(new { Message = responseMessage });

            return BadRequest(new { Message = responseMessage });
        }
    }
}
