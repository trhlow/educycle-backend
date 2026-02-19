using EduCycle.Domain.Enums;

namespace EduCycle.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Role Role { get; set; } = Role.User;
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }

    // === OAuth Fields ===
    public string? GoogleId { get; set; }
    public string? FacebookId { get; set; }
    public string? MicrosoftId { get; set; }

    // === Email Verification ===
    public bool IsEmailVerified { get; set; } = false;
    public string? EmailVerificationToken { get; set; }
    public DateTime? EmailVerificationTokenExpiry { get; set; }

    // === Password Reset ===
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }

    // === Phone Verification ===
    public string? Phone { get; set; }
    public bool PhoneVerified { get; set; } = false;
}
