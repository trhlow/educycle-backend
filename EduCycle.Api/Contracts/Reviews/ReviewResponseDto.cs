namespace EduCycle.Contracts.Reviews;

public class ReviewResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Username { get; set; }

    public Guid? ProductId { get; set; }
    public Guid? TargetUserId { get; set; }
    public Guid? TransactionId { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
