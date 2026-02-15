namespace EduCycle.Contracts.Reviews;

public class ReviewResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
