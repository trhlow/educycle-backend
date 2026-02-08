namespace EduCycle.Api.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    public int SellerId { get; set; }
    public User Seller { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public bool IsAvailable { get; set; } = true;
}
