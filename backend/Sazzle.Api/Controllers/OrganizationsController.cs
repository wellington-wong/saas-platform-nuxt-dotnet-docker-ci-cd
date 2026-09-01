using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sazzle.Application.Organizations;

namespace Sazzle.Api.Controllers;

[ApiController]
[Route("api/orgs")]
[Authorize]
public class OrganizationsController : ControllerBase
{
    private readonly OrganizationService _organizationService;

    public OrganizationsController(OrganizationService organizationService)
    {
        _organizationService = organizationService;

    }

    public record CreateOrganizationRequest(string Name, string Slug);

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrganizationRequest request)
    {
        var userId = GetCurrentUserId();
        try
        {

            var organization = await _organizationService.CreateOrganizationAsync(
                request.Name, request.Slug, userId);

            return Ok(new { organization.Id, organization.Name, organization.Slug });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }

    }

    private Guid GetCurrentUserId()
    {

        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst("sub")?.Value;

        if (sub is null || !Guid.TryParse(sub, out var userId))
            throw new UnauthorizedAccessException("Invalid or missing user identity in token.");

        return userId;
    }


    [HttpGet("me")]
    public async Task<IActionResult> GetMyOrganizations()
    {
        var userId = GetCurrentUserId();
        var orgs = await _organizationService.GetMyOrganizationsAsync(userId);

        return Ok(orgs.Select(o => new { o.Id, o.Name, o.Slug }));
    }



	[HttpGet("{orgId}/members")]
	[Authorize(Policy = "members:view")]
	public async Task<IActionResult> GetMembers(Guid orgId)
	{
		var members = await _organizationService.GetMembersAsync(orgId);
		return Ok(members);
	}



	[HttpDelete("{orgId}/members/{userId}")]
	[Authorize(Policy = "members:remove")]
	public async Task<IActionResult> RemoveMember(Guid orgId, Guid userId)
	{
		if (userId == GetCurrentUserId())
			return BadRequest(new { error = "You cannot remove yourself from the organization." });

		var removed = await _organizationService.RemoveMemberAsync(orgId, userId);
		if (!removed) return NotFound();

		return NoContent();

	}
}