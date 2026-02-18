using EduCycle.Application.Interfaces;
using EduCycle.Common.Exceptions;
using EduCycle.Contracts.Auth;
using EduCycle.Domain.Entities;
using EduCycle.Domain.Enums;
using EduCycle.Infrastructure.Authentication;
using EduCycle.Infrastructure.Repositories;

namespace EduCycle.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new BadRequestException("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Role.User,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user),
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedException("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials");

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user),
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponse> SocialLoginAsync(SocialLoginRequest request)
    {
        // Map provider to a demo email (in production, validate OAuth token)
        var email = request.Email; 
        
        // Use user provided email if available, otherwise mock based on provider
        if (string.IsNullOrEmpty(email))
        {
            email = request.Provider?.ToLower() switch
            {
                "microsoft" => "student@university.edu.vn",
                "google" => "student@gmail.com",
                "facebook" => "student@facebook.com",
                _ => throw new BadRequestException($"Unsupported provider: {request.Provider}")
            };
        }
        else if (request.Provider?.ToLower() == "microsoft" && !email.EndsWith(".edu.vn"))
        {
             // Optional: Enforce edu.vn for Microsoft if we want to be strict, 
             // but user might use personal microsoft account.
             // For now, let's allow it but prefer edu.vn for students.
        }

        var username = email.Split('@')[0];

        // Find existing user or create new one
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                Role = Role.User,
                CreatedAt = DateTime.UtcNow
            };
            await _userRepository.AddAsync(user);
        }

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user),
            Role = user.Role.ToString()
        };
    }

    public async Task<bool> VerifyPhoneAsync(Guid userId, VerifyPhoneRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new BadRequestException("User not found");

        // In production, validate OTP via SMS provider (Twilio, etc.)
        // For dev/demo, accept any OTP
        user.Phone = request.Phone;
        user.PhoneVerified = true;
        await _userRepository.UpdateAsync(user);

        return true;
    }
}
