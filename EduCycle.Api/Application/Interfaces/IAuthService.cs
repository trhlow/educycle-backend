using EduCycle.Contracts.Auth;

namespace EduCycle.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> SocialLoginAsync(SocialLoginRequest request);
}
