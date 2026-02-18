using EduCycle.Domain.Entities;

namespace EduCycle.Domain.Entities;

public class Review
{
    public Guid Id { get; set; }

    // User viết review
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Product được review (optional if user review)
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }

    // User được review (User-to-User)
    public Guid? TargetUserId { get; set; }
    public User? TargetUser { get; set; }

    public Guid? TransactionId { get; set; }

    public int Rating { get; set; }
    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
