using dotnetapp.Models;
using System.Threading.Tasks;
namespace dotnetapp.Services
{
    public interface IAuthService
    {
        // Simplified registration flow:
        Task<(int, string)> Register(User model);

        // Simplified login flow:
        Task<(int, string)> Login(LoginModel model);
    }
}
