namespace EduCycle.Contracts.Auth;

public class AuthResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Role { get; set; } = null!;
}
