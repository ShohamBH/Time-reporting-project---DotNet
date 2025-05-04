using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TimeReporting.Core.Interface;
using TimeReporting.Core.DTOs;
using TimeReporting.Core.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims; // כדי לגשת לזהות המשתמש הנוכחי

namespace TimeReporting.Controllers
{
    [Authorize] // דורש אימות עבור כל הפונקציות בבקר
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "UserAndAdmin")]
        public ActionResult<IEnumerable<object>> Get() // שינוי סוג ההחזרה ל-object
        {
            if (User.IsInRole("Admin"))
            {
                List<User> users = _userService.GetFullUsers().ToList();
                if (users != null)
                    return Ok(users); // מנהל מקבל List<User>
            }
            else
            {
                List<DTOUser> dtoUsers = _userService.GetUsers().ToList();
                return Ok(dtoUsers); // משתמש רגיל מקבל List<DTOUser>
            }
            return NotFound();
        }


        [HttpGet("{id}")]
        [Authorize(Policy = "UserAndAdmin")]
        public ActionResult<object> Get(string id) // שינוי סוג ההחזרה ל-object
        {
            _logger.LogInformation($"Getting user with ID: {id}.");

            User fullUser = _userService.GetFullUser(id);
            if (fullUser == null)
            {
                _logger.LogWarning($"User with ID: {id} not found.");
                return NotFound("User ID not found");
            }

            if (User.IsInRole("Admin"))
            {
                return Ok(fullUser); // מנהל מקבל User מלא
            }
            else
            {
                // אם המשתמש הנוכחי מנסה לקבל את המידע של עצמו, החזר User מלא
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id)
                {
                    return Ok(fullUser);
                }
                else
                {
                    DTOUser dtoUser = _userService.GetUser(id);
                    return Ok(dtoUser); // משתמש רגיל מקבל DTOUser עבור משתמשים אחרים
                }
            }
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")] // רק מנהלים מורשים להוסיף משתמשים
        public async Task<ActionResult> Post([FromBody] User u)
        {
            _logger.LogInformation($"Attempting to add user: {u.FirstName} {u.LastName} with ID: {u.Id}.");
            try
            {
                await _userService.AddUserAsync(u);
                _logger.LogInformation($"User {u.FirstName} {u.LastName} with ID: {u.Id} added successfully.");
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, $"Failed to add user. {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Failed to add user. {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Failed to add user. {ex.Message}");
                return Conflict(ex.Message); // שימוש ב-Conflict עבור משתמש קיים
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding a user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserAndAdmin")] // רק משתמשים ועובדים מורשים
        public async Task<ActionResult> Put(string id, [FromBody] User u)
        {
            _logger.LogInformation($"Attempting to update user with ID: {id}.");
            var temp = new User
            {
                Id = u.Id,
                City = u.City,
                Email = u.Email,
                Phone = u.Phone,
                Password = u.Password,
                LastName = u.LastName,
                FirstName = u.FirstName,
                Role = u.Role
            };

            if (User.IsInRole("Admin"))
            {
                if (await _userService.UpdateUserAsync(id, temp))
                {
                    _logger.LogInformation($"User with ID {id} updated successfully by admin.");
                    return Ok();
                }
            }
            else
            {
                // משתמש רגיל יכול לעדכן רק את הפרטים של עצמו (ללא שינוי תפקיד)
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id)
                {
                    var userToUpdate = new User
                    {
                        Id = u.Id,
                        City = u.City,
                        Email = u.Email,
                        Phone = u.Phone,
                        Password = u.Password,
                        LastName = u.LastName,
                        FirstName = u.FirstName
                    };
                    if (await _userService.UpdateUserAsync(id, userToUpdate))
                    {
                        _logger.LogInformation($"User with ID {id} updated successfully by regular user.");
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Failed to update user.");
                    }
                }
                else
                {
                    return Forbid("You are not authorized to update this user.");
                }
            }

            _logger.LogWarning($"User with ID {id} not found for update.");
            return NotFound("Id User not found");
        }

        [HttpDelete("deleteById/{id}")]
        [Authorize(Policy = "UserAndAdmin")] // רק משתמשים ועובדים מורשים
        public async Task<ActionResult> Delete(string id)
        {
            _logger.LogInformation($"Attempting to delete user with ID: {id}.");
            // משתמש רגיל יכול למחוק רק את עצמו (בדרך כלל לא מאפשרים את זה, אבל לצורך הדוגמה)
            if (!User.IsInRole("Admin") && User.FindFirstValue(ClaimTypes.NameIdentifier) != id)
            {
                return Forbid("You are not authorized to delete this user.");
            }

            if (await _userService.DeleteUserAsync(id))
            {
                _logger.LogInformation($"User with ID {id} deleted successfully.");
                return Ok();
            }

            _logger.LogWarning($"User with ID {id} not found for deletion.");
            return NotFound("Id user not found");
        }
    }
}