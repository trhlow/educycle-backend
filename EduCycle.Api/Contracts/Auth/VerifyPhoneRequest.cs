namespace EduCycle.Contracts.Auth;

public class VerifyPhoneRequest
{
    public string Phone { get; set; } = null!;
    public string Otp { get; set; } = null!;
}
