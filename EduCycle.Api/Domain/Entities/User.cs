namespace EduCycle.Api.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public double AvgRating { get; set; } = 0;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
