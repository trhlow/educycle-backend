namespace EduCycle.Contracts.Transactions;

public class CreateTransactionRequest
{
    public Guid SellerId { get; set; }
    public decimal Amount { get; set; }
}
