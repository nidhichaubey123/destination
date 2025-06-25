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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailAddress) || !Regex.IsMatch(request.EmailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Invalid email address.");
            }

            if (await context.Users.AnyAsync(u => u.emailAddress == request.EmailAddress))
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

                createdOn = DateTime.Now,
                UserIsActive = false
            };

            context.Users.Add(newUser);

            try
            {
                await context.SaveChangesAsync();

                var response = new UserRegisterResponse
                {
                    UserId = newUser.userId
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Message = "Failed to save user to database",
                    Error = ex.Message
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.emailAddress == request.EmailAddress);

            if (user == null || !PasswordHelper.VerifyPassword(user.password, request.Password))
            {
                var unauthorizedResponse = new ErrorResponse { Message = "Invalid credentials" };
                return Unauthorized(unauthorizedResponse);
            }


            // ⛔ Check if user is blocked
            if (!user.UserIsActive)
            {
                var blockedResponse = new ErrorResponse
                {
                    Message = "YOUR ACCOUNT IS BLOCKED. PLEASE CONTACT ADMIN"
                };
                return Unauthorized(blockedResponse);
            }

            // ✅ Activate user after successful login
            user.lastLoggedOn = DateTime.Now;
            user.UserIsActive = true;



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

            var operations = await (
                from ur in context.UserRoles
                join ro in context.RoleOperations on ur.RoleId equals ro.RoleId
                join op in context.Operations on ro.OperationId equals op.OperationId
                where ur.UserId == user.userId
                   && ur.IsActive
                   && ro.IsActive
                   && op.OperationIsActive
                select op.OperationName
            ).Distinct().ToListAsync();

            var loginResponse = new LoginResponse
            {
                SessionId = newSession.sessionId,
                Operations = operations ?? new List<string>(),
                    FirstName = user.firstName,
                LastName = user.lastName

            };

          
            return Ok(loginResponse);
        }

        [HttpGet]
  
        public async Task<IActionResult> GetUsers()
        {
            var users = await context.Users
                    .OrderBy(u => u.firstName)
                    .ThenBy(u => u.lastName)
                    .Select(u => new UserDto
                    {
                        userId = u.userId,
                        emailAddress = u.emailAddress,
                        firstName = u.firstName,
                        middleName = u.middleName,
                        lastName = u.lastName,
                        mobileNo = u.mobileNo,
                        lastLoggedOn = u.lastLoggedOn,
                        UserIsActive = u.UserIsActive,

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

            var response = new DeleteUserResponse { Message = "User and roles deleted successfully" };
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto user)
        {
            if (id != user.UserId)
            {
                var mismatchResponse = new ErrorResponse { Message = "Mismatched user ID." };
                return BadRequest(mismatchResponse);
            }

            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.EmailAddress))
            {
                var requiredFieldsResponse = new ErrorResponse { Message = "First name, last name, and email are required." };
                return BadRequest(requiredFieldsResponse);
            }

            if (!Regex.IsMatch(user.EmailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                var invalidEmailResponse = new ErrorResponse { Message = "Invalid email format." };
                return BadRequest(invalidEmailResponse);
            }

            if (!string.IsNullOrWhiteSpace(user.MobileNo) && !Regex.IsMatch(user.MobileNo, @"^\d{10}$"))
            {
                var invalidMobileResponse = new ErrorResponse { Message = "Mobile number must be exactly 10 digits." };
                return BadRequest(invalidMobileResponse);
            }

            var emailExists = await context.Users.AnyAsync(u => u.emailAddress == user.EmailAddress && u.userId != id);
            if (emailExists)
            {
                var emailExistsResponse = new ErrorResponse { Message = "Email already exists." };
                return BadRequest(emailExistsResponse);
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

            var updateResponse = new UpdateUserResponse { Message = "User updated successfully" };
            return Ok(updateResponse);
        }


        [HttpPut("ChangeStatus/{id}")]
        public IActionResult ChangeStatus(int id, [FromBody] UserStatusUpdateDto statusUpdate)
        {
            var user = context.Users.FirstOrDefault(u => u.userId == id);
            if (user == null)
                return NotFound();

            user.UserIsActive = statusUpdate.UserIsActive;
            context.SaveChanges();

            return Ok(new { message = "Status updated successfully." });
        }


        [HttpDelete("DeleteSession/{sessionId}")]
        public IActionResult DeleteSession(Guid sessionId)
        {
            var session = context.UserSessions.FirstOrDefault(s => s.sessionId == sessionId);
            if (session != null)
            {
                context.UserSessions.Remove(session);
                context.SaveChanges();
            }
            return Ok();
        }


    }

    // Response Classes
    public class UserRegisterResponse
    {
        public int UserId { get; set; }
    }

   

    public class DeleteUserResponse
    {
        public string Message { get; set; }
    }

    public class UpdateUserResponse
    {
        public string Message { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Error { get; set; }
    }


   


}