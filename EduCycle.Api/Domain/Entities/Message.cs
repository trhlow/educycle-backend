namespace EduCycle.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }

    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;

    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
