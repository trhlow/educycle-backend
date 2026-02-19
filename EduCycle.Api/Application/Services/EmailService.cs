using System.Net;
using System.Net.Mail;
using EduCycle.Application.Interfaces;

namespace EduCycle.Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendOtpEmailAsync(string toEmail, string otp)
    {
        var smtpHost = _config["Email:SmtpHost"] ?? "smtp.gmail.com";
        var smtpPort = int.Parse(_config["Email:SmtpPort"] ?? "587");
        var smtpUsername = _config["Email:SmtpUsername"]
            ?? throw new InvalidOperationException("Email:SmtpUsername is not configured");
        var smtpPassword = _config["Email:SmtpPassword"]
            ?? throw new InvalidOperationException("Email:SmtpPassword is not configured");
        var fromEmail = _config["Email:FromEmail"] ?? smtpUsername;
        var fromName = _config["Email:FromName"] ?? "EduCycle";

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = true,
        };

        var message = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = $"EduCycle - Mã xác thực OTP: {otp}",
            Body = $"""
                <div style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto; padding: 32px; background: #f8f9fa; border-radius: 12px;">
                    <h2 style="color: #2d3436; text-align: center;">🎓 EduCycle</h2>
                    <p>Xin chào,</p>
                    <p>Mã xác thực OTP của bạn là:</p>
                    <div style="text-align: center; margin: 24px 0;">
                        <span style="font-size: 32px; font-weight: bold; letter-spacing: 8px; color: #6c5ce7; background: #fff; padding: 16px 32px; border-radius: 8px; border: 2px dashed #6c5ce7;">
                            {otp}
                        </span>
                    </div>
                    <p>Mã này có hiệu lực trong <strong>5 phút</strong>.</p>
                    <p style="color: #636e72; font-size: 12px;">Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email này.</p>
                </div>
                """,
            IsBodyHtml = true,
        };
        message.To.Add(toEmail);

        await client.SendMailAsync(message);
    }
}
