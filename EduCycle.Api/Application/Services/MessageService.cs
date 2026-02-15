using EduCycle.Application.Interfaces;
using EduCycle.Contracts.Messages;
using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Repositories;

namespace EduCycle.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;

    public MessageService(IMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MessageResponse>> GetByTransactionIdAsync(Guid transactionId)
    {
        var messages = await _repository.GetByTransactionIdAsync(transactionId);
        return messages.Select(MapToResponse).ToList();
    }

    public async Task<MessageResponse> SendAsync(Guid transactionId, SendMessageRequest request, Guid senderId)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            SenderId = senderId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(message);

        return MapToResponse(message);
    }

    private static MessageResponse MapToResponse(Message m) => new()
    {
        Id = m.Id,
        TransactionId = m.TransactionId,
        SenderId = m.SenderId,
        SenderName = m.Sender?.Username,
        Content = m.Content,
        CreatedAt = m.CreatedAt
    };
}
