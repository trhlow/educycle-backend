namespace EduCycle.Contracts.Auth;

public class SocialLoginRequest
{
    public string Provider { get; set; } = null!;
    public string? Email { get; set; }
    public string? ProviderId { get; set; }
}
