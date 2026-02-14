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

    public async Task<ReviewResponse> CreateAsync(
        CreateReviewRequest request,
        Guid userId)
    {
        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = request.ProductId,
            Rating = request.Rating,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(review);

        return MapToResponse(review);
    }

    public async Task<ReviewResponse> GetByIdAsync(Guid id)
    {
        var review = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Review with id '{id}' not found");

        return MapToResponse(review);
    }

    public async Task<List<ReviewResponse>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

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

    private static ReviewResponse MapToResponse(Review r) => new()
    {
        Id = r.Id,
        UserId = r.UserId,
        ProductId = r.ProductId,
        Rating = r.Rating,
        Content = r.Content
    };
}
