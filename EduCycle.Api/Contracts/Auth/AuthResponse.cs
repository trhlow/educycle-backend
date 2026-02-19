namespace EduCycle.Contracts.Auth;

public record AuthResponse
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Token { get; init; } = null!;
    public string Role { get; init; } = null!;
    public bool IsEmailVerified { get; init; }
    public string? Message { get; init; }
}
