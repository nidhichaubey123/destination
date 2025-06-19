using DMCPortal.API.Entities;
using DMCPortal.API.Entities.Helpers;
using DMCPortal.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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


            // Fetch user by email
            var user = context.Users.FirstOrDefault(u => u.emailAddress == request.EmailAddress);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid credentials" });  // Always return JSON
            }

            bool isValid = PasswordHelper.VerifyPassword(user.password, request.Password);
            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }


            user.lastLoggedOn = DateTime.Now;

            // Your session logic (as before)
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

            // Remove roles first
            var roles = context.UserRoles.Where(ur => ur.UserId == id);
            context.UserRoles.RemoveRange(roles);

            // Then remove user
            context.Users.Remove(user);

            await context.SaveChangesAsync();
            return Ok(new { message = "User and roles deleted successfully" });
        }





    }
}



