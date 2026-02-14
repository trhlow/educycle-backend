using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduCycle.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _context.Transactions.FindAsync(id);
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions.AsNoTracking().ToListAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }
}
