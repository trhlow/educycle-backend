namespace EduCycle.Contracts.Messages;

public class MessageResponse
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public Guid SenderId { get; set; }
    public string? SenderName { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
