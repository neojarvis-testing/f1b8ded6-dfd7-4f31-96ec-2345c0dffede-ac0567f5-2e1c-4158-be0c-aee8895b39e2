using System;
using System.Collections.Concurrent;
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
using Microsoft.AspNetCore.Identity; //framework
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
        private readonly IEmailService _emailService;

        // Use thread-safe dictionaries for in-memory OTP and pending registration storage.
        private static readonly ConcurrentDictionary<string, (string otp, DateTime expiry)> _otpStore =
            new ConcurrentDictionary<string, (string, DateTime)>();

        private static readonly ConcurrentDictionary<string, User> _pendingRegistrations =
            new ConcurrentDictionary<string, User>();

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
        }

        // ----------------------------------------------------
        // Registration Flow with OTP (Send and Verify OTP)
        // ----------------------------------------------------

        // STEP 1: Send an OTP when the user registers.
        public async Task<(int, string)> SendRegistrationOtp(User model)
        {
            // Check if a user with the provided email already exists.
            var foundUser = await _userManager.FindByEmailAsync(model.Email);
            if (foundUser != null)
            {
                Console.WriteLine($"[Registration OTP] User already exists for email: {model.Email}");
                return (0, "User already exists");
            }

            // Normalize the email.
            var normalizedEmail = model.Email.Trim().ToLower();

            // Generate a 6-digit OTP and set a 5-minute expiration (UTC).
            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(5);

            // Store OTP and pending registration details.
            _otpStore[normalizedEmail] = (otp, expiry);
            _pendingRegistrations[normalizedEmail] = model;

            // Log a generic message without exposing OTP details.
            Console.WriteLine($"[Registration OTP] OTP generated for '{normalizedEmail}' with expiry at {expiry.ToLocalTime()}.");

            // Send the OTP via email.
            await _emailService.SendEmailAsync(model.Email, "Your Registration OTP",
                $"Your OTP for registration is: {otp}. It is valid for 5 minutes.");

            return (1, "OTP sent successfully.");
        }

        
        public async Task<(int, string)> VerifyRegistrationOtp(string email, string otp)
        {
            var normalizedEmail = email.Trim().ToLower();

            if (!_otpStore.TryGetValue(normalizedEmail, out var storedTuple))
            {
                Console.WriteLine($"[Registration OTP] OTP not found for '{normalizedEmail}'.");
                return (0, "OTP expired or not found");
            }

            // Destructure the stored tuple.
            var (storedOtp, expiry) = storedTuple;
            Console.WriteLine($"[Registration OTP] OTP verification attempt for '{normalizedEmail}'.");

            // Check for OTP expiry.
            if (DateTime.UtcNow > expiry)
            {
                Console.WriteLine($"[Registration OTP] OTP expired for '{normalizedEmail}'.");
                _otpStore.TryRemove(normalizedEmail, out _);
                _pendingRegistrations.TryRemove(normalizedEmail, out _);
                return (0, "OTP expired. Please request a new one.");
            }

            // Compare the stored OTP with the submitted OTP.
            if (storedOtp != otp)
            {
                Console.WriteLine($"[Registration OTP] OTP mismatch for '{normalizedEmail}'.");
                return (0, "Invalid OTP");
            }

            if (!_pendingRegistrations.TryGetValue(normalizedEmail, out var model))
            {
                Console.WriteLine($"[Registration OTP] No pending registration for '{normalizedEmail}'.");
                return (0, "No pending registration found");
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
                Console.WriteLine($"[Registration OTP] User creation failed for '{normalizedEmail}'.");
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

            // Remove the temporary OTP and pending registration details.
            _pendingRegistrations.TryRemove(normalizedEmail, out _);
            _otpStore.TryRemove(normalizedEmail, out _);

            Console.WriteLine($"[Registration OTP] User '{normalizedEmail}' created successfully.");
            return (1, "User created successfully!");
        }
       
        // Login Flow with OTP and Password Check

        // Standard login to generate a JWT token.
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

        // Send OTP for login – only after verifying the user's password.
        public async Task<(int, string)> SendOtp(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                Console.WriteLine($"[Login OTP] Invalid email: {model.Email}");
                return (0, "Invalid email");
            }

            // Validates the user's password using SignInManager.
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!signInResult.Succeeded)
            {
                Console.WriteLine($"[Login OTP] Invalid password for '{model.Email}'.");
                return (0, "Invalid password");
            }

            var normalizedEmail = model.Email.Trim().ToLower();
            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(5);
            _otpStore[normalizedEmail] = (otp, expiry);

            Console.WriteLine($"[Login OTP] OTP generated for '{normalizedEmail}' with expiry at {expiry.ToLocalTime()}.");

            // Send the OTP via email.
            await _emailService.SendEmailAsync(model.Email, "Your OTP for Login",
                $"Your OTP for login is: {otp}. It is valid for 5 minutes.");
            return (1, "OTP sent successfully.");
        }

        // Verify OTP for login.
        public async Task<(int, string)> VerifyOtp(string email, string otp)
        {
            var normalizedEmail = email.Trim().ToLower();

            if (_otpStore.TryGetValue(normalizedEmail, out var storedTuple))
            {
                var (storedOtp, expiry) = storedTuple;
                Console.WriteLine($"[Login OTP] Received OTP verification attempt for '{normalizedEmail}'.");

                if (DateTime.UtcNow > expiry)
                {
                    Console.WriteLine($"[Login OTP] OTP expired for '{normalizedEmail}'.");
                    _otpStore.TryRemove(normalizedEmail, out _);
                    return (0, "OTP expired. Please request a new one.");
                }

                if (storedOtp == otp)
                {
                    Console.WriteLine($"[Login OTP] OTP verified successfully for '{normalizedEmail}'.");
                    _otpStore.TryRemove(normalizedEmail, out _);
                    return (1, "OTP verified successfully.");
                }
                else
                {
                    Console.WriteLine($"[Login OTP] OTP mismatch for '{normalizedEmail}'.");
                    return (0, "Invalid OTP");
                }
            }
            Console.WriteLine($"[Login OTP] OTP not found or expired for '{normalizedEmail}'.");
            return (0, "OTP not found or expired.");
        }

       //Token Generation
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var secretKey = _configuration["JWT:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),  
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
