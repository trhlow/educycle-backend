using EduCycle.Contracts.Products;

namespace EduCycle.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request, Guid userId);
    Task<ProductResponse> GetByIdAsync(Guid id);
    Task<List<ProductResponse>> GetAllAsync();
    Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
