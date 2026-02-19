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
    private readonly IProductRepository _productRepository;

    public TransactionService(
        ITransactionRepository repository,
        IProductRepository productRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
    }

    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, Guid buyerId)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            BuyerId = buyerId,
            SellerId = request.SellerId,
            Amount = request.Amount,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(transaction);

        var full = await _repository.GetByIdAsync(transaction.Id);
        return MapToResponse(full ?? transaction);
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

    public async Task<List<TransactionResponse>> GetMyTransactionsAsync(Guid userId)
    {
        var list = await _repository.GetByUserIdAsync(userId);
        return list.Select(MapToResponse).ToList();
    }

    public async Task<TransactionResponse> UpdateStatusAsync(Guid id, UpdateTransactionStatusRequest request)
    {
        var transaction = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Transaction with id '{id}' not found");

        transaction.Status = Enum.Parse<TransactionStatus>(request.Status);
        await _repository.UpdateAsync(transaction);

        return MapToResponse(transaction);
    }

    public async Task<object> GenerateOtpAsync(Guid id)
    {
        var transaction = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Transaction with id '{id}' not found");

        var otp = Random.Shared.Next(100000, 999999).ToString();
        transaction.OtpCode = otp;
        transaction.OtpExpiresAt = DateTime.UtcNow.AddMinutes(10);
        await _repository.UpdateAsync(transaction);

        return new { otp };
    }

    public async Task VerifyOtpAsync(Guid id, string otp)
    {
        var transaction = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Transaction with id '{id}' not found");

        if (transaction.OtpCode != otp || transaction.OtpExpiresAt < DateTime.UtcNow)
            throw new BadRequestException("Invalid or expired OTP");

        transaction.OtpCode = null;
        transaction.OtpExpiresAt = null;
        transaction.Status = TransactionStatus.Completed;
        await _repository.UpdateAsync(transaction);

        // Auto-delete (mark as Sold) the product
        var product = await _productRepository.GetByIdAsync(transaction.ProductId);
        if (product != null)
        {
            product.Status = ProductStatus.Sold;
            await _productRepository.UpdateAsync(product);
        }
    }

    public async Task<TransactionResponse> ConfirmReceiptAsync(Guid id)
    {
        var transaction = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Transaction with id '{id}' not found");

        transaction.Status = TransactionStatus.Completed;
        await _repository.UpdateAsync(transaction);

        // Auto-delete (mark as Sold) the product
        var product = await _productRepository.GetByIdAsync(transaction.ProductId);
        if (product != null)
        {
            product.Status = ProductStatus.Sold;
            await _productRepository.UpdateAsync(product);
        }

        return MapToResponse(transaction);
    }

    private static TransactionResponse MapToResponse(Transaction t) => new()
    {
        Id = t.Id,
        Buyer = t.Buyer != null ? new TransactionUserDto
        {
            Id = t.Buyer.Id.ToString(),
            Username = t.Buyer.Username,
            Email = t.Buyer.Email
        } : null,
        Seller = t.Seller != null ? new TransactionUserDto
        {
            Id = t.Seller.Id.ToString(),
            Username = t.Seller.Username,
            Email = t.Seller.Email
        } : null,
        Product = t.Product != null ? new TransactionProductDto
        {
            Id = t.Product.Id.ToString(),
            Name = t.Product.Name,
            Price = t.Product.Price,
            ImageUrl = t.Product.ImageUrl
        } : null,
        Amount = t.Amount,
        Status = t.Status.ToString(),
        BuyerConfirmed = t.BuyerConfirmed,
        SellerConfirmed = t.SellerConfirmed,
        CreatedAt = t.CreatedAt
    };
}
