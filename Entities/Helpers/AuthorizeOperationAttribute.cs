using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMCPortal.API.Helpers
{
    public class AuthorizeOperationAttribute : Attribute, IAuthorizationFilter
    {
        public string OperationName { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var dbContext = context.HttpContext.RequestServices.GetService(typeof(DMCPortalDBContext)) as DMCPortalDBContext;

            var sessionIdHeader = context.HttpContext.Request.Headers["SessionId"].ToString();

            if (string.IsNullOrWhiteSpace(sessionIdHeader) || !Guid.TryParse(sessionIdHeader, out Guid sessionGuid))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "SessionId header missing or invalid." });
                return;
            }

            var session = dbContext.UserSessions.FirstOrDefault(s => s.sessionId == sessionGuid && !s.isExpired);
            if (session == null)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Session not found or expired." });
                return;
            }

            var userId = session.userId;

            var operationId = dbContext.Operations
                .Where(o => o.OperationName == OperationName)
                .Select(o => o.OperationId)
                .FirstOrDefault();

            if (operationId == 0)
            {
                context.Result = new ForbidResult("Operation not found.");
                return;
            }

            var hasOperation = dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(dbContext.RoleOperations,
                      ur => ur.RoleId,
                      ro => ro.RoleId,
                      (ur, ro) => ro)
                .Any(ro => ro.OperationId == operationId && ro.IsActive);

            if (!hasOperation)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "You do not have permission to perform this operation." });
            }
        }
    }

}
