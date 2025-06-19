using DMCPortal.API.Entities;
using DMCPortal.API.Entities.Helpers;
using DMCPortal.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DMCPortalDBContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(DMCPortalDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        [AuthorizeOperation(OperationName = "CanCreateUser")]
        public IActionResult RegisterUser([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailAddress) || !Regex.IsMatch(request.EmailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Invalid email address.");
            }

            if (context.Users.Any(u => u.emailAddress == request.EmailAddress))
            {
                var problem = new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "EmailAddress", new[] { "Email already exists." } }
                });
                return BadRequest(problem);
            }

            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                return BadRequest("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                return BadRequest("Last name is required.");
            }

            if (!string.IsNullOrWhiteSpace(request.MobileNo) && !Regex.IsMatch(request.MobileNo, @"^\d{10}$"))
            {
                return BadRequest("Mobile number must be exactly 10 digits.");
            }

            string hashedPassword = PasswordHelper.HashPassword(request.Password);

            var newUser = new User
            {
                emailAddress = request.EmailAddress,
                password = hashedPassword,
                firstName = request.FirstName,
                middleName = request.MiddleName,
                lastName = request.LastName,
                mobileNo = request.MobileNo,
                createdOn = DateTime.Now
            };

            context.Users.Add(newUser);
            context.SaveChangesAsync();

            return Ok(new { userId = newUser.userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = context.Users.FirstOrDefault(u => u.emailAddress == request.EmailAddress);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid credentials" });
            }

            bool isValid = PasswordHelper.VerifyPassword(user.password, request.Password);
            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            user.lastLoggedOn = DateTime.Now;

            var oldSessions = await context.UserSessions
                .Where(s => s.userId == user.userId && !s.isExpired)
                .ToListAsync();

            foreach (var session in oldSessions)
            {
                session.isExpired = true;
                session.expiredOn = DateTime.Now;
            }

            var newSession = new UserSession
            {
                sessionId = Guid.NewGuid(),
                userId = user.userId,
                sessionIPAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown",
                sessionHostName = Environment.MachineName,
                createdOn = DateTime.Now,
                isExpired = false
            };

            context.UserSessions.Add(newSession);
            await context.SaveChangesAsync();

            return Ok(new { sessionId = newSession.sessionId });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await context.Users
                .Select(u => new UserDto
                {
                    userId = u.userId,
                    emailAddress = u.emailAddress,
                    firstName = u.firstName,
                    middleName = u.middleName,
                    lastName = u.lastName,
                    mobileNo = u.mobileNo,
                    lastLoggedOn = u.lastLoggedOn,
                    Roles = context.UserRoles
                                .Where(ur => ur.UserId == u.userId)
                                .Select(ur => ur.Role.RoleName)
                                .ToList()
                }).ToListAsync();

            return Ok(users);
        }

        [HttpDelete("{id}")]
       
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var roles = context.UserRoles.Where(ur => ur.UserId == id);
            context.UserRoles.RemoveRange(roles);

            context.Users.Remove(user);

            await context.SaveChangesAsync();
            return Ok(new { message = "User and roles deleted successfully" });
        }

        [HttpPut("{id}")]
     
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto user)
        {
            if (id != user.UserId)
                return BadRequest("Mismatched user ID.");

            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.EmailAddress))
            {
                return BadRequest(new { message = "First name, last name, and email are required." });
            }

            if (!Regex.IsMatch(user.EmailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest(new { message = "Invalid email format." });
            }

            if (!string.IsNullOrWhiteSpace(user.MobileNo) && !Regex.IsMatch(user.MobileNo, @"^\d{10}$"))
            {
                return BadRequest(new { message = "Mobile number must be exactly 10 digits." });
            }

            var emailExists = await context.Users.AnyAsync(u => u.emailAddress == user.EmailAddress && u.userId != id);
            if (emailExists)
            {
                return BadRequest(new { message = "Email already exists." });
            }

            var existing = await context.Users.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.firstName = user.FirstName;
            existing.middleName = user.MiddleName;
            existing.lastName = user.LastName;
            existing.emailAddress = user.EmailAddress;
            existing.mobileNo = user.MobileNo;

            await context.SaveChangesAsync();
            return Ok(new { message = "User updated successfully" });
        }
    }
}
