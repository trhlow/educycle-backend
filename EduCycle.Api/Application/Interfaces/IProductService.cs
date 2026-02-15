using EduCycle.Contracts.Products;

namespace EduCycle.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request, Guid userId);
    Task<ProductResponse> GetByIdAsync(Guid id);
    Task<List<ProductResponse>> GetAllAsync();
    Task<List<ProductResponse>> GetAllForAdminAsync();
    Task<List<ProductResponse>> GetPendingAsync();
    Task<List<ProductResponse>> GetMyProductsAsync(Guid userId);
    Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
    Task<ProductResponse> ApproveAsync(Guid id);
    Task<ProductResponse> RejectAsync(Guid id);
}
