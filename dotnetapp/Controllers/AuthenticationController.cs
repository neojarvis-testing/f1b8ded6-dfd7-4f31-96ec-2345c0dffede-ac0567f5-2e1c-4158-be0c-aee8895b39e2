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

        // ----- Login Endpoints -----

        // Endpoint to send OTP for login.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid login request." });

            var (statusCode, responseMessage) = await _authService.SendOtp(model);
            if (statusCode == 1)
                return Ok(new { Message = responseMessage });
            return Unauthorized(new { Message = responseMessage });
        }

        // Endpoint to verify login OTP and then return a token.
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpModel model)
        {
            var (statusCode, responseMessage) = await _authService.VerifyOtp(model.Email, model.Otp);
            if (statusCode == 1)
            {
                // If OTP is verified, call Login to generate a token.
                var (loginStatus, tokenOrMsg) = await _authService.Login(new LoginModel { Email = model.Email });
                if (loginStatus == 1)
                    return Ok(new { token = tokenOrMsg });
                return Unauthorized(new { Message = tokenOrMsg });
            }
            return Unauthorized(new { Message = responseMessage });
        }

        // ----- Registration Endpoints -----

        // Endpoint to send OTP when registering.
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid registration request." });

            var (statusCode, responseMessage) = await _authService.SendRegistrationOtp(model);
            if (statusCode == 1)
                return Ok(new { Message = responseMessage });
            return BadRequest(new { Message = responseMessage });
        }

        // Endpoint to verify the registration OTP and complete registration.
        [HttpPost("verify-registration-otp")]
        public async Task<IActionResult> VerifyRegistrationOtp([FromBody] OtpModel model)
        {
            var (statusCode, responseMessage) = await _authService.VerifyRegistrationOtp(model.Email, model.Otp);
            if (statusCode == 1)
                return Ok(new { Message = responseMessage });
            return BadRequest(new { Message = responseMessage });
        }
    }
}
