using EduCycle.Contracts.Products;

namespace EduCycle.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request, Guid userId);
    Task<List<ProductResponse>> GetAllAsync();
}
