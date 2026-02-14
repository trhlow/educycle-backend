using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(int id);
    Task<List<Category>> GetAllAsync();
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
}
