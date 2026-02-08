namespace EduCycle.Api.Domain.Entities;

public class Message
{
    public int Id { get; set; }

    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;

    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
