namespace EduCycle.Api.Domain.Entities;

public class Review
{
    public int Id { get; set; }

    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;

    public int ReviewerId { get; set; }
    public User Reviewer { get; set; } = null!;

    public int Rating { get; set; } // 1–5
}
