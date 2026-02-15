namespace EduCycle.Contracts.Products;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public List<string>? ImageUrls { get; set; }
    public string? Category { get; set; }
    public string? CategoryName { get; set; }
    public int? CategoryId { get; set; }
    public string? Condition { get; set; }
    public string? ContactNote { get; set; }
    public Guid UserId { get; set; }
    public string? SellerName { get; set; }
    public Guid? SellerId { get; set; }
    public string Status { get; set; } = null!;
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
