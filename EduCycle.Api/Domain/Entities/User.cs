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
    public string? Phone { get; set; }
    public bool PhoneVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}
