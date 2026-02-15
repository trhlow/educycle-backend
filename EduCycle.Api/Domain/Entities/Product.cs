using EduCycle.Domain.Enums;

namespace EduCycle.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    // Multiple images stored as JSON array
    public string? ImageUrls { get; set; }

    public string? Category { get; set; }
    public string? Condition { get; set; }
    public string? ContactNote { get; set; }

    public int? CategoryId { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ProductStatus Status { get; set; } = ProductStatus.Pending;

    public DateTime CreatedAt { get; set; }
}
