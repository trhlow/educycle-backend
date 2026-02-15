using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Repositories;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<List<Transaction>> GetAllAsync();
    Task<List<Transaction>> GetByUserIdAsync(Guid userId);
    Task UpdateAsync(Transaction transaction);
}
