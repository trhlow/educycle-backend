using EduCycle.Domain.Entities;

namespace EduCycle.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }

    public Guid BuyerId { get; set; }
    public User Buyer { get; set; } = null!;

    public Guid SellerId { get; set; }
    public User Seller { get; set; } = null!;

    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
