using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Repositories;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<List<Transaction>> GetAllAsync();
}
