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
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user),
            Role = user.Role.ToString()
        };
    }
}
