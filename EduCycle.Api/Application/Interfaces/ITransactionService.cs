using EduCycle.Contracts.Transactions;

namespace EduCycle.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, Guid buyerId);
    Task<List<TransactionResponse>> GetAllAsync();
}
