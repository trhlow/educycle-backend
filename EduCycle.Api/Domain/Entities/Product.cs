namespace EduCycle.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
