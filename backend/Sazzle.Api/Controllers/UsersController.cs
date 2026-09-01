using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sazzle.Application.Common.Interfaces;
namespace Sazzle.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;


    public UsersController(IUserRepository users) => _users = users;

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = GetCurrentUserId();
        var user = await _users.GetByIdAsync(userId);


        if (user is null) return NotFound();

        return Ok(new { user.Id, user.Email, user.FullName, user.EmailConfirmed, user.CreatedAt });
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (sub is null || !Guid.TryParse(sub, out var userId))

        {
            throw new UnauthorizedAccessException("Invalid or missing user identity in token.");
        }

        return userId;
    }
}