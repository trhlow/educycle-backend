using EduCycle.Application.Interfaces;
using EduCycle.Common.Exceptions;
using EduCycle.Contracts.Categories;
using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Repositories;

namespace EduCycle.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name
        };

        await _repository.AddAsync(category);

        return MapToResponse(category);
    }

    public async Task<CategoryResponse> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Category with id '{id}' not found");

        return MapToResponse(category);
    }

    public async Task<List<CategoryResponse>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(MapToResponse).ToList();
    }

    public async Task<CategoryResponse> UpdateAsync(int id, CreateCategoryRequest request)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Category with id '{id}' not found");

        category.Name = request.Name;

        await _repository.UpdateAsync(category);

        return MapToResponse(category);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Category with id '{id}' not found");

        await _repository.DeleteAsync(category);
    }

    private static CategoryResponse MapToResponse(Category c) => new()
    {
        Id = c.Id,
        Name = c.Name
    };
}
