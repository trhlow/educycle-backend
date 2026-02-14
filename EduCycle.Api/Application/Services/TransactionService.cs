using EduCycle.Application.Interfaces;
using EduCycle.Common.Exceptions;
using EduCycle.Contracts.Transactions;
using EduCycle.Domain.Entities;
using EduCycle.Domain.Enums;
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
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(transaction);

        return MapToResponse(transaction);
    }

    public async Task<TransactionResponse> GetByIdAsync(Guid id)
    {
        var transaction = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Transaction with id '{id}' not found");

        return MapToResponse(transaction);
    }

    public async Task<List<TransactionResponse>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        return list.Select(MapToResponse).ToList();
    }

    public async Task<TransactionResponse> UpdateStatusAsync(
        Guid id,
        UpdateTransactionStatusRequest request)
    {
        var transaction = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Transaction with id '{id}' not found");

        transaction.Status = Enum.Parse<TransactionStatus>(request.Status);

        await _repository.UpdateAsync(transaction);

        return MapToResponse(transaction);
    }

    private static TransactionResponse MapToResponse(Transaction t) => new()
    {
        Id = t.Id,
        BuyerId = t.BuyerId,
        SellerId = t.SellerId,
        Amount = t.Amount,
        Status = t.Status.ToString(),
        CreatedAt = t.CreatedAt
    };
}
