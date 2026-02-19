using EduCycle.Application.Interfaces;
using EduCycle.Common.Exceptions;
using EduCycle.Contracts.Reviews;
using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Repositories;

namespace EduCycle.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _repository;

    public ReviewService(IReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReviewResponseDto> CreateAsync(CreateReviewRequest request, Guid userId)
    {
        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = request.ProductId,
            TargetUserId = request.TargetUserId,
            TransactionId = request.TransactionId,
            Rating = request.Rating,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(review);

        return MapToResponse(review);
    }

    public async Task<ReviewResponseDto> GetByIdAsync(Guid id)
    {
        var review = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Review with id '{id}' not found");

        return MapToResponse(review);
    }

    public async Task<List<ReviewResponseDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(MapToResponse).ToList();
    }

    public async Task<List<ReviewResponseDto>> GetByProductIdAsync(Guid productId)
    {
        var list = await _repository.GetByProductIdAsync(productId);
        return list.Select(MapToResponse).ToList();
    }

    public async Task<List<ReviewResponseDto>> GetByTargetUserIdAsync(Guid userId)
    {
        // Fallback: GetAll and filter if repository doesn't support it directly
        var all = await _repository.GetAllAsync();
        var list = all.Where(r => r.TargetUserId == userId).ToList();
        return list.Select(MapToResponse).ToList();
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var review = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Review with id '{id}' not found");

        if (review.UserId != userId)
            throw new UnauthorizedException("You can only delete your own reviews");

        await _repository.DeleteAsync(review);
    }

    private static ReviewResponseDto MapToResponse(Review r) => new()
    {
        Id = r.Id,
        UserId = r.UserId,
        Username = MaskUsername(r.User?.Username),
        ReviewerName = MaskUsername(r.User?.Username),
        ProductId = r.ProductId,
        TargetUserId = r.TargetUserId,
        TransactionId = r.TransactionId,
        Rating = r.Rating,
        Content = r.Content,
        CreatedAt = r.CreatedAt
    };

    private static string MaskUsername(string? username)
    {
        if (string.IsNullOrEmpty(username)) return "Ngu***";
        if (username.Length <= 3) return username + "***";
        return username[..3] + "***" + username[^1];
    }
}
