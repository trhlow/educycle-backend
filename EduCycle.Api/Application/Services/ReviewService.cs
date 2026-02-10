using EduCycle.Application.Interfaces;
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
            Content = request.Content
        };

        await _repository.AddAsync(review);

        return new ReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            ProductId = review.ProductId,
            Rating = review.Rating,
            Content = review.Content
        };
    }

    public async Task<List<ReviewResponse>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        return list.Select(r => new ReviewResponse
        {
            Id = r.Id,
            UserId = r.UserId,
            ProductId = r.ProductId,
            Rating = r.Rating,
            Content = r.Content
        }).ToList();
    }
}
