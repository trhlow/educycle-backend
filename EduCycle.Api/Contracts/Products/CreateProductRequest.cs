namespace EduCycle.Contracts.Products;

public class CreateProductRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public List<string>? ImageUrls { get; set; }
    public string? Category { get; set; }
    public string? Condition { get; set; }
    public string? ContactNote { get; set; }
    public int? CategoryId { get; set; }
}
