using EduCycle.Contracts.Transactions;

namespace EduCycle.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, Guid buyerId);
    Task<TransactionResponse> GetByIdAsync(Guid id);
    Task<List<TransactionResponse>> GetAllAsync();
    Task<TransactionResponse> UpdateStatusAsync(Guid id, UpdateTransactionStatusRequest request);
}
