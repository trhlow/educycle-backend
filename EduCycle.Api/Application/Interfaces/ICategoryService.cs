using EduCycle.Contracts.Categories;

namespace EduCycle.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
    Task<CategoryResponse> GetByIdAsync(int id);
    Task<List<CategoryResponse>> GetAllAsync();
    Task<CategoryResponse> UpdateAsync(int id, CreateCategoryRequest request);
    Task DeleteAsync(int id);
}
