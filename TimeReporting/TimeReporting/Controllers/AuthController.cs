using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using TimeReporting.Core.Interface;
using TimeReporting.Core.Entities;
using Microsoft.Extensions.Logging;

namespace TimeReporting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            _logger.LogInformation($"Attempting login for user ID: {model.Id}");

            User user = null;

            // **בדיקה מיוחדת עבור מנהל "ADMIN" עם סיסמה "123456"**
            if (model.Id?.ToUpper() == "ADMIN" && model.Password == "123456")
            {
                user = new User // יוצרים אובייקט User עם תפקיד מנהל (לא חייבים לאחזר מהמסד)
                {
                    Id = "ADMIN",
                    FirstName = "Admin",
                    LastName = "User",
                    Role = Role.Admin
                };
            }
            else
            {
                // ניסיון התחברות רגיל עבור משתמשים אחרים (כולל מנהלים נוספים)
                try
                {
                    user = _userService.GetFullUser(model.Id);
                    if (user == null || !VerifyPassword(model.Password, user.Password))
                    {
                        _logger.LogWarning($"Login failed for user ID: {model.Id} - Invalid credentials.");
                        return Unauthorized();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning($"Login failed for user ID: {model.Id} - User not found: {ex.Message}");
                    return Unauthorized();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An unexpected error occurred during login for user ID: {model.Id}");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            // אם המשתמש אותנטיקציה עברה בהצלחה
            if (user != null)
            {
                var token = GenerateJwtToken(user);
                _logger.LogInformation($"Login successful for user ID: {user.Id}, token generated.");
                return Ok(new { Token = token });
            }

            // אם אף תנאי התחברות לא התקיים
            _logger.LogWarning($"Login failed for user ID: {model.Id} - No matching user found.");
            return Unauthorized();
        }

        private bool VerifyPassword(string providedPassword, string storedHashedPassword)
        {
            // **חשוב:** יש ליישם כאן לוגיקה של השוואת סיסמאות מגובבות עם מלח.
            // זהו יישום פשוט ולא מאובטח לצורך הדוגמה בלבד.
            return providedPassword == storedHashedPassword;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // שינוי זמן התפוגה ל-30 דקות
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }
}