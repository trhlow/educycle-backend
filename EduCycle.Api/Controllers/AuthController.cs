using EduCycle.Application.Interfaces;
using EduCycle.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EduCycle.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        return Ok(await _authService.RegisterAsync(request));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        return Ok(await _authService.LoginAsync(request));
    }

    [HttpPost("social-login")]
    public async Task<IActionResult> SocialLogin(SocialLoginRequest request)
    {
        return Ok(await _authService.SocialLoginAsync(request));
    }
}
