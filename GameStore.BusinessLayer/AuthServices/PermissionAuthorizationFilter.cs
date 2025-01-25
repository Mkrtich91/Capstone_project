using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.BusinessLayer.AuthServices
{
    public class PermissionAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var permissionNameAttribute = context.ActionDescriptor.EndpointMetadata
                .OfType<PermissionNameAttribute>()
                .FirstOrDefault();

            if (permissionNameAttribute != null)
            {
                var requiredPermission = permissionNameAttribute.PermissionName;

                var userClaims = context.HttpContext.User;

                if (!userClaims.HasClaim(c => c.Type == "Permission" && c.Value == requiredPermission))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
