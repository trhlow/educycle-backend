using EduCycle.Domain.Entities;
using EduCycle.Domain.Enums;

namespace EduCycle.Infrastructure.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetByStatusAsync(ProductStatus status);
    Task<List<Product>> GetByUserIdAsync(Guid userId);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
