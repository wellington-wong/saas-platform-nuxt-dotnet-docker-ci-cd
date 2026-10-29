using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sazzle.Application.Common.Interfaces;

namespace Sazzle.Api.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RolesController(IRoleRepository roleRepository) => _roleRepository = roleRepository;

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleRepository.GetSystemRolesAsync();
        return Ok(roles.Select(role => new { role.Id, role.Name }));
    }
}
