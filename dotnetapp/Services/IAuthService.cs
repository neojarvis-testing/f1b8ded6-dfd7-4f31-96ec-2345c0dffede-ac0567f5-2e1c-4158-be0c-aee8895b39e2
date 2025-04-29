using dotnetapp.Models;
using System.Threading.Tasks;

namespace dotnetapp.Services
{
    public interface IAuthService
    {
        // Registration with OTP:
        Task<(int, string)> SendRegistrationOtp(User model);
        Task<(int, string)> VerifyRegistrationOtp(string email, string otp);
        
        // Login flow:
        Task<(int, string)> Login(LoginModel model);
        Task<(int, string)> SendOtp(LoginModel model);
        Task<(int, string)> VerifyOtp(string email, string otp);
    }
}
