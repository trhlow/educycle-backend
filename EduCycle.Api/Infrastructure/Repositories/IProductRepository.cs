using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetAllAsync();
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
