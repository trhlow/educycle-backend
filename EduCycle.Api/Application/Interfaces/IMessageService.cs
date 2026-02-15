using EduCycle.Contracts.Messages;

namespace EduCycle.Application.Interfaces;

public interface IMessageService
{
    Task<List<MessageResponse>> GetByTransactionIdAsync(Guid transactionId);
    Task<MessageResponse> SendAsync(Guid transactionId, SendMessageRequest request, Guid senderId);
}
