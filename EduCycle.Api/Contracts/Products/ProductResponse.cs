namespace EduCycle.Contracts.Products;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
