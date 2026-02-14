using EduCycle.Application.Services;
using EduCycle.Common.Exceptions;
using EduCycle.Contracts.Auth;
using EduCycle.Domain.Entities;
using EduCycle.Domain.Enums;
using EduCycle.Infrastructure.Authentication;
using EduCycle.Infrastructure.Repositories;
using Moq;

namespace EduCycle.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IJwtTokenGenerator> _jwtMock;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _jwtMock = new Mock<IJwtTokenGenerator>();
        _sut = new AuthService(_userRepoMock.Object, _jwtMock.Object);
    }

    // ===== REGISTER =====

    [Fact]
    public async Task RegisterAsync_ShouldReturnToken_WhenEmailIsNew()
    {
        _userRepoMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _jwtMock.Setup(j => j.GenerateToken(It.IsAny<User>()))
            .Returns("fake-jwt-token");

        var request = new RegisterRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123"
        };

        var result = await _sut.RegisterAsync(request);

        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("fake-jwt-token", result.Token);
        Assert.Equal("User", result.Role);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        _userRepoMock.Setup(r => r.ExistsByEmailAsync("existing@example.com"))
            .ReturnsAsync(true);

        var request = new RegisterRequest
        {
            Username = "testuser",
            Email = "existing@example.com",
            Password = "Password123"
        };

        await Assert.ThrowsAsync<BadRequestException>(() => _sut.RegisterAsync(request));
    }

    // ===== LOGIN =====

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
            Role = Role.User,
            CreatedAt = DateTime.UtcNow
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(user);

        _jwtMock.Setup(j => j.GenerateToken(user))
            .Returns("fake-jwt-token");

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123"
        };

        var result = await _sut.LoginAsync(request);

        Assert.NotNull(result);
        Assert.Equal("fake-jwt-token", result.Token);
        Assert.Equal(user.Id, result.UserId);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync("notfound@example.com"))
            .ReturnsAsync((User?)null);

        var request = new LoginRequest
        {
            Email = "notfound@example.com",
            Password = "Password123"
        };

        await Assert.ThrowsAsync<UnauthorizedException>(() => _sut.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordIsWrong()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
            Role = Role.User,
            CreatedAt = DateTime.UtcNow
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(user);

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "WrongPassword"
        };

        await Assert.ThrowsAsync<UnauthorizedException>(() => _sut.LoginAsync(request));
    }
}
