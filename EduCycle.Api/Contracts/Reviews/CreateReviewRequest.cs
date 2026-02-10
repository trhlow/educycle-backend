namespace EduCycle.Contracts.Reviews;

public class CreateReviewRequest
{
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = null!;
}
