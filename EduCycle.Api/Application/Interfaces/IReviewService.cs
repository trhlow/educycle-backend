using EduCycle.Contracts.Reviews;

namespace EduCycle.Application.Interfaces;

public interface IReviewService
{
    Task<ReviewResponseDto> CreateAsync(CreateReviewRequest request, Guid userId);
    Task<ReviewResponseDto> GetByIdAsync(Guid id);
    Task<List<ReviewResponseDto>> GetAllAsync();
    Task<List<ReviewResponseDto>> GetByProductIdAsync(Guid productId);
    Task<List<ReviewResponseDto>> GetByTargetUserIdAsync(Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
