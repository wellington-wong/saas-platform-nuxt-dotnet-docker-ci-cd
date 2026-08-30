using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Sazzle.Application.Authorization;
using Sazzle.Application.Common.Interfaces;

namespace Sazzle.Api.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IOrganizationRepository _organizations;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public PermissionAuthorizationHandler(IOrganizationRepository organizations, IHttpContextAccessor httpContextAccessor)
    {
        _organizations = organizations;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionRequirement requirement)
    {

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;

        var orgIdRaw = httpContext.Request.RouteValues["orgId"]?.ToString();
        if (orgIdRaw is null || !Guid.TryParse(orgIdRaw, out var orgId))
            return; // no orgId in route -> can't evaluate, deny by default (fail closed)

        var sub = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? context.User.FindFirst("sub")?.Value;

        if (sub is null || !Guid.TryParse(sub, out var userId))
            return;

        var hasPermission = await _organizations.UserHasPermissionAsync(userId, orgId, requirement.Permission);
        if (hasPermission)
            context.Succeed(requirement);
        // if false, we simple don't call Succeed() - this fails the requirement, which is correct (fail closed)
    }
}