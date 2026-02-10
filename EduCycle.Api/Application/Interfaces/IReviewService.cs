using EduCycle.Contracts.Reviews;

namespace EduCycle.Application.Interfaces;

public interface IReviewService
{
    Task<ReviewResponse> CreateAsync(CreateReviewRequest request, Guid userId);
    Task<List<ReviewResponse>> GetAllAsync();
}
