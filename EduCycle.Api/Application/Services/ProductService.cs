using EduCycle.Application.Interfaces;
using EduCycle.Contracts.Products;
using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Repositories;

namespace EduCycle.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        Guid userId)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            UserId = product.UserId
        };
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            UserId = p.UserId
        }).ToList();
    }
}
