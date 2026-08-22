using Microsoft.AspNetCore.Mvc;
using Sazzle.Application.Auth;


namespace Sazzle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService) => _authService = authService;

    public record RegisterRequest(string Email, string Password, string FullName);

    public record LoginRequest(string Email, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {

            var user = await _authService.RegisterAsync(request.Email, request.Password, request.FullName);
            return Ok(new { user.Id, user.Email, user.FullName });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]

    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException ex)
        {

            return Unauthorized(new { error = ex.Message });
        }
    }
    
}