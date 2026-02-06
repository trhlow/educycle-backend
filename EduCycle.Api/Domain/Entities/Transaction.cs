using EduCycle.Api.Domain.Enums;

namespace EduCycle.Api.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int BuyerId { get; set; }
    public User Buyer { get; set; } = null!;

    public int SellerId { get; set; }
    public User Seller { get; set; } = null!;

    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
}
