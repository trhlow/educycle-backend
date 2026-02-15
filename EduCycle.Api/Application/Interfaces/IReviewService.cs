using EduCycle.Contracts.Reviews;

namespace EduCycle.Application.Interfaces;

public interface IReviewService
{
    Task<ReviewResponse> CreateAsync(CreateReviewRequest request, Guid userId);
    Task<ReviewResponse> GetByIdAsync(Guid id);
    Task<List<ReviewResponse>> GetAllAsync();
    Task<List<ReviewResponse>> GetByProductIdAsync(Guid productId);
    Task DeleteAsync(Guid id, Guid userId);
}
