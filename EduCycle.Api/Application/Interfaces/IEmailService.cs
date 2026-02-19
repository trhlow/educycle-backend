namespace EduCycle.Application.Interfaces;

public interface IEmailService
{
    Task SendOtpEmailAsync(string toEmail, string otp);
    Task SendWelcomeEmailAsync(string toEmail, string username);
    Task SendPasswordResetEmailAsync(string toEmail, string resetToken);
}
