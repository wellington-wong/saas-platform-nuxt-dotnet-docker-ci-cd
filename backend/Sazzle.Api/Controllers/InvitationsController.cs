using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sazzle.Application.Organizations;
namespace Sazzle.Api.Controllers;

[ApiController]
[Route("api/orgs")]
[Authorize]

public class InvitationsController : ControllerBase
{


    private readonly InvitationService _invitationService;
    public InvitationsController(InvitationService invitationService) => _invitationService = invitationService;

    public record InviteRequest(string Email, Guid RoleId);

    [HttpPost("{orgId}/invitations")]
    [Authorize(Policy = "members:invite")]
    public async Task<IActionResult> Invite(Guid orgId, InviteRequest request)

    {

		if (request.RoleId == Guid.Empty)
			return BadRequest(new { error = "roleId is required." });

		if (string.IsNullOrWhiteSpace(request.Email))
			return BadRequest(new { error = "email is required." });
	
        var userId = GetCurrentUserId();
        var invitation = await _invitationService.InviteAsync(orgId, request.Email, request.RoleId, userId);
        return Ok(new { invitation.Id, invitation.Email, invitation.Token, invitation.ExpiresAt });
    }


    [HttpPost("invitations/{token}/accept")]
    public async Task<IActionResult> Accept(string token)
    {
        var userId = GetCurrentUserId();
        try
        {
            await _invitationService.AcceptAsync(token, userId);
            return Ok(new { message = "Invitation accepted." });
        }

        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (sub is null || !Guid.TryParse(sub, out var userId))

            throw new UnauthorizedAccessException("Invalid or missing user identity in token.");

        return userId;
    }
    
}