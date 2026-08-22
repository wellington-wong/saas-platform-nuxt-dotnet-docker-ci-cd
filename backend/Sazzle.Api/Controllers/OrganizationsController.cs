using Microsoft.AspNetCore.Mvc;
using Sazzle.Application.Organizations;
namespace Sazzle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly OrganizationService _organizationService;

    public OrganizationsController(OrganizationService organizationService)
    {
        _organizationService = organizationService;

    }

    public record CreateOrganizationRequest(string Name, string Slug, Guid CreatorUserId);

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrganizationRequest request)
    {
        try
        {

            var organization = await _organizationService.CreateOrganizationAsync(
                request.Name, request.Slug, request.CreatorUserId);

            return Ok(new { organization.Id, organization.Name, organization.Slug });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }

    }
}