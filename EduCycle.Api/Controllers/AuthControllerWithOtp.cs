using EduCycle.Application.Interfaces;
using EduCycle.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCycle.Api.Controllers;

/// <summary>
/// Alternative auth controller with full OTP support.
/// Route: api/authwithotp — use this if you want to keep the original AuthController separate.
/// The main AuthController already includes verify-otp and resend-otp endpoints.
/// </summary>
[ApiController]
[Route("api/authwithotp")]
public class AuthControllerWithOtp : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthControllerWithOtp(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        return Ok(await _authService.LoginAsync(request));
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
    {
        await _authService.VerifyOtpAsync(request);
        return Ok(new { message = "Email verified successfully!" });
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(ResendOtpRequest request)
    {
        await _authService.ResendOtpAsync(request);
        return Ok(new { message = "OTP resent successfully!" });
    }

    [HttpPost("social-login")]
    public async Task<IActionResult> SocialLogin(SocialLoginRequest request)
    {
        return Ok(await _authService.SocialLoginAsync(request));
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
