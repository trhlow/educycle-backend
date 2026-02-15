namespace EduCycle.Contracts.Transactions;

public class TransactionResponse
{
    public Guid Id { get; set; }
    public TransactionUserDto? Buyer { get; set; }
    public TransactionUserDto? Seller { get; set; }
    public TransactionProductDto? Product { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class TransactionUserDto
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class TransactionProductDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}
