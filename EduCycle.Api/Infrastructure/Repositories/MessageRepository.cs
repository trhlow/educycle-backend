using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduCycle.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetByTransactionIdAsync(Guid transactionId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Where(m => m.TransactionId == transactionId)
            .OrderBy(m => m.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }
}
