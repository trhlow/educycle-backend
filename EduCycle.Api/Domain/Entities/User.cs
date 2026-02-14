using EduCycle.Domain.Enums;

namespace EduCycle.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Role Role { get; set; } = Role.User;
    public DateTime CreatedAt { get; set; }
}
