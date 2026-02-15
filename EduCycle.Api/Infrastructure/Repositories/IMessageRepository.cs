using EduCycle.Domain.Entities;

namespace EduCycle.Infrastructure.Repositories;

public interface IMessageRepository
{
    Task AddAsync(Message message);
    Task<List<Message>> GetByTransactionIdAsync(Guid transactionId);
}
