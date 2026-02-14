using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Repositories;

public interface IReviewRepository
{
    Task AddAsync(Review review);
    Task<Review?> GetByIdAsync(Guid id);
    Task<List<Review>> GetAllAsync();
    Task DeleteAsync(Review review);
}
