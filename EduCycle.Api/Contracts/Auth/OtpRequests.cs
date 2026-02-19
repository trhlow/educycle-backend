namespace EduCycle.Contracts.Auth;

public record VerifyOtpRequest
{
    public string Email { get; init; } = null!;
    public string Otp { get; init; } = null!;
}

public record ResendOtpRequest
{
    public string Email { get; init; } = null!;
}
