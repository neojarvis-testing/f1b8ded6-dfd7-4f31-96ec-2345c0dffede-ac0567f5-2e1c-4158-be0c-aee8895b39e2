using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace dotnetapp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        // Simplified registration method.
        public async Task<(int, string)> Register(User model)
        {
            // Check if a user with the provided email already exists.
            var foundUser = await _userManager.FindByEmailAsync(model.Email);
            if (foundUser != null)
            {
                Console.WriteLine($"[Register] User already exists for email: {model.Email}");
                return (0, "User already exists");
            }

            // Create the ASP.NET Identity user.
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                Console.WriteLine($"[Register] User creation failed for '{model.Email}'.");
                return (0, "User creation failed! Please check user details and try again.");
            }

            // Ensure the role exists before assigning it.
            if (!await _roleManager.RoleExistsAsync(model.UserRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.UserRole));
            }
            await _userManager.AddToRoleAsync(user, model.UserRole);

            // Add a custom user record in your application-specific database.
            var customUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                MobileNumber = model.MobileNumber,
                Password = model.Password,
                UserRole = model.UserRole
            };
            _context.Users.Add(customUser);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[Register] User '{model.Email}' created successfully.");
            return (1, "User created successfully!");
        }

        // Simplified login to generate a JWT token.
        public async Task<(int, string)> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                Console.WriteLine($"[Login] Invalid email: {model.Email}");
                return (0, "Invalid email");
            }

            var customUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (customUser == null)
            {
                Console.WriteLine($"[Login] User not found in the database: {model.Email}");
                return (0, "User not found in the database");
            }

            var role = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, customUser.UserId.ToString()),
                new Claim(ClaimTypes.Role, role.FirstOrDefault() ?? "User"),
            };

            var token = GenerateToken(claims);
            Console.WriteLine($"[Login] JWT token generated for '{model.Email}'.");
            return (1, token);
        }

        // Helper: Token Generation
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            // WARNING: Ensure that the JWT secret key is obtained from a secure source (e.g., an environment variable or a secret manager)
            var secretKey = _configuration["JWT:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
