using EduCycle.Application.Interfaces;
using EduCycle.Contracts.Transactions;
using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Repositories;

namespace EduCycle.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;

    public TransactionService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<TransactionResponse> CreateAsync(
        CreateTransactionRequest request,
        Guid buyerId)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            BuyerId = buyerId,
            SellerId = request.SellerId,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(transaction);

        return new TransactionResponse
        {
            Id = transaction.Id,
            BuyerId = transaction.BuyerId,
            SellerId = transaction.SellerId,
            Amount = transaction.Amount
        };
    }

    public async Task<List<TransactionResponse>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        return list.Select(t => new TransactionResponse
        {
            Id = t.Id,
            BuyerId = t.BuyerId,
            SellerId = t.SellerId,
            Amount = t.Amount
        }).ToList();
    }
}
