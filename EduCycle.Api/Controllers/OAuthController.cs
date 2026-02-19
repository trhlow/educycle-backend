using EduCycle.Application.Interfaces;
using EduCycle.Domain.Entities;
using EduCycle.Domain.Enums;
using EduCycle.Infrastructure.Authentication;
using EduCycle.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCycle.Api.Controllers;

[ApiController]
[Route("api/oauth")]
public class OAuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IConfiguration _config;

    public OAuthController(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IConfiguration config)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _config = config;
    }

    // ===== GOOGLE =====

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback))
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return BadRequest("Google authentication failed");

        var claims = result.Principal!.Claims;
        var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        var user = await FindOrCreateOAuthUser(googleId, email, name, "google");
        var token = _jwtTokenGenerator.GenerateToken(user);

        var frontendUrl = _config["Frontend:Url"] ?? "http://localhost:3000";
        return Redirect($"{frontendUrl}/oauth-callback?token={token}");
    }

    // ===== FACEBOOK =====

    [HttpGet("facebook-login")]
    public IActionResult FacebookLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(FacebookCallback))
        };
        return Challenge(properties, FacebookDefaults.AuthenticationScheme);
    }

    [HttpGet("facebook-callback")]
    public async Task<IActionResult> FacebookCallback()
    {
        var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return BadRequest("Facebook authentication failed");

        var claims = result.Principal!.Claims;
        var facebookId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        var user = await FindOrCreateOAuthUser(facebookId, email, name, "facebook");
        var token = _jwtTokenGenerator.GenerateToken(user);

        var frontendUrl = _config["Frontend:Url"] ?? "http://localhost:3000";
        return Redirect($"{frontendUrl}/oauth-callback?token={token}");
    }

    // ===== MICROSOFT =====

    [HttpGet("microsoft-login")]
    public IActionResult MicrosoftLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(MicrosoftCallback))
        };
        return Challenge(properties, MicrosoftAccountDefaults.AuthenticationScheme);
    }

    [HttpGet("microsoft-callback")]
    public async Task<IActionResult> MicrosoftCallback()
    {
        var result = await HttpContext.AuthenticateAsync(MicrosoftAccountDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return BadRequest("Microsoft authentication failed");

        var claims = result.Principal!.Claims;
        var microsoftId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        var user = await FindOrCreateOAuthUser(microsoftId, email, name, "microsoft");
        var token = _jwtTokenGenerator.GenerateToken(user);

        var frontendUrl = _config["Frontend:Url"] ?? "http://localhost:3000";
        return Redirect($"{frontendUrl}/oauth-callback?token={token}");
    }

    // ===== HELPER =====

    private async Task<User> FindOrCreateOAuthUser(
        string? providerId, string? email, string? name, string provider)
    {
        // Try to find by provider ID first
        User? user = provider switch
        {
            "google" => providerId != null ? await _userRepository.GetByGoogleIdAsync(providerId) : null,
            "facebook" => providerId != null ? await _userRepository.GetByFacebookIdAsync(providerId) : null,
            "microsoft" => providerId != null ? await _userRepository.GetByMicrosoftIdAsync(providerId) : null,
            _ => null
        };

        // Fall back to email lookup
        if (user == null && !string.IsNullOrEmpty(email))
            user = await _userRepository.GetByEmailAsync(email);

        if (user != null)
            return user;

        // Create new user
        user = new User
        {
            Id = Guid.NewGuid(),
            Username = name ?? email?.Split('@')[0] ?? "user",
            Email = email ?? $"{providerId}@{provider}.oauth",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
            Role = Role.User,
            CreatedAt = DateTime.UtcNow,
            IsEmailVerified = true
        };

        switch (provider)
        {
            case "google": user.GoogleId = providerId; break;
            case "facebook": user.FacebookId = providerId; break;
            case "microsoft": user.MicrosoftId = providerId; break;
        }

        await _userRepository.AddAsync(user);
        return user;
    }
}
