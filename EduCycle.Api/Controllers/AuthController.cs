using EduCycle.Application.Interfaces;
using EduCycle.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
    {
        var result = await _authService.VerifyOtpAsync(request);
        return Ok(new { message = "Email verified successfully!" });
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(ResendOtpRequest request)
    {
        var result = await _authService.ResendOtpAsync(request);
        return Ok(new { message = "OTP resent successfully!" });
    }

    [Authorize]
    [HttpPost("verify-phone")]
    public async Task<IActionResult> VerifyPhone(VerifyPhoneRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _authService.VerifyPhoneAsync(userId, request);
        return Ok(new { success = result });
    }
}
