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
        var subject = $"EduCycle - Mã xác thực OTP: {otp}";
        var body = $"""
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
            """;

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string username)
    {
        var frontendUrl = _config["Frontend:Url"] ?? "http://localhost:5173";
        var subject = "🎓 Chào mừng bạn đến với EduCycle!";
        var body = $"""
            <div style="font-family: Arial, sans-serif; max-width: 520px; margin: 0 auto; padding: 32px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 16px;">
                <div style="background: #fff; border-radius: 12px; padding: 32px;">
                    <h2 style="color: #2d3436; text-align: center; margin-bottom: 24px;">🎓 Chào mừng đến EduCycle!</h2>
                    <p>Xin chào <strong>{username}</strong>,</p>
                    <p>Tài khoản của bạn đã được xác thực thành công! 🎉</p>
                    <p>Bạn có thể bắt đầu:</p>
                    <ul style="line-height: 2;">
                        <li>📚 Tìm kiếm sách & tài liệu học tập</li>
                        <li>📝 Đăng bán sách không còn sử dụng</li>
                        <li>🤝 Giao dịch trực tiếp với sinh viên khác</li>
                        <li>⭐ Đánh giá uy tín người bán/mua</li>
                    </ul>
                    <div style="text-align: center; margin-top: 24px;">
                        <a href="{frontendUrl}/products"
                           style="display: inline-block; padding: 14px 32px; background: #6c5ce7; color: #fff; text-decoration: none; border-radius: 8px; font-weight: bold; font-size: 16px;">
                            Khám phá ngay →
                        </a>
                    </div>
                    <p style="color: #636e72; font-size: 12px; margin-top: 24px; text-align: center;">
                        EduCycle — Nền tảng trao đổi tài liệu sinh viên
                    </p>
                </div>
            </div>
            """;

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetToken)
    {
        var frontendUrl = _config["Frontend:Url"] ?? "http://localhost:5173";
        var resetLink = $"{frontendUrl}/reset-password?token={resetToken}&email={Uri.EscapeDataString(toEmail)}";
        var subject = "EduCycle - Đặt lại mật khẩu";
        var body = $"""
            <div style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto; padding: 32px; background: #f8f9fa; border-radius: 12px;">
                <h2 style="color: #2d3436; text-align: center;">🔒 Đặt lại mật khẩu</h2>
                <p>Xin chào,</p>
                <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản EduCycle của bạn.</p>
                <div style="text-align: center; margin: 24px 0;">
                    <a href="{resetLink}"
                       style="display: inline-block; padding: 14px 32px; background: #e17055; color: #fff; text-decoration: none; border-radius: 8px; font-weight: bold; font-size: 16px;">
                        Đặt lại mật khẩu →
                    </a>
                </div>
                <p style="font-size: 13px;">Hoặc copy link: <code style="background: #fff; padding: 4px 8px; border-radius: 4px; word-break: break-all;">{resetLink}</code></p>
                <p>Link này có hiệu lực trong <strong>30 phút</strong>.</p>
                <p style="color: #636e72; font-size: 12px;">Nếu bạn không yêu cầu đặt lại mật khẩu, hãy bỏ qua email này.</p>
            </div>
            """;

        await SendEmailAsync(toEmail, subject, body);
    }

    // ===== PRIVATE HELPER =====

    private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
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
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true,
        };
        message.To.Add(toEmail);

        await client.SendMailAsync(message);
    }
}
