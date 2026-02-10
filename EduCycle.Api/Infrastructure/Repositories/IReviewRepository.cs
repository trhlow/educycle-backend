using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Repositories;

public interface IReviewRepository
{
    Task AddAsync(Review review);
    Task<List<Review>> GetAllAsync();
}
