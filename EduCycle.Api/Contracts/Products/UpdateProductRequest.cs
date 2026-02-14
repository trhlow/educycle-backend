namespace EduCycle.Contracts.Products;

public class UpdateProductRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
}
