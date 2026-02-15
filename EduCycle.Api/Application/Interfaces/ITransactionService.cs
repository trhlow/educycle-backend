using EduCycle.Contracts.Transactions;

namespace EduCycle.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, Guid buyerId);
    Task<TransactionResponse> GetByIdAsync(Guid id);
    Task<List<TransactionResponse>> GetAllAsync();
    Task<List<TransactionResponse>> GetMyTransactionsAsync(Guid userId);
    Task<TransactionResponse> UpdateStatusAsync(Guid id, UpdateTransactionStatusRequest request);
    Task<object> GenerateOtpAsync(Guid id);
    Task VerifyOtpAsync(Guid id, string otp);
    Task<TransactionResponse> ConfirmReceiptAsync(Guid id);
}
