using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<List<Product>> GetAllAsync();
}
