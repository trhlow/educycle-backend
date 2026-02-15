using System.Text.Json;
using EduCycle.Application.Interfaces;
using EduCycle.Common.Exceptions;
using EduCycle.Contracts.Products;
using EduCycle.Domain.Entities;
using EduCycle.Domain.Enums;
using EduCycle.Infrastructure.Repositories;

namespace EduCycle.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;

    public ProductService(IProductRepository productRepository, IReviewRepository reviewRepository)
    {
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, Guid userId)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl ?? request.ImageUrls?.FirstOrDefault(),
            ImageUrls = request.ImageUrls != null ? JsonSerializer.Serialize(request.ImageUrls) : null,
            Category = request.Category,
            Condition = request.Condition,
            ContactNote = request.ContactNote,
            CategoryId = request.CategoryId,
            UserId = userId,
            Status = ProductStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);

        return MapToResponse(product);
    }

    public async Task<ProductResponse> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product with id '{id}' not found");

        var reviews = await _reviewRepository.GetByProductIdAsync(id);
        return MapToResponse(product, reviews);
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetByStatusAsync(ProductStatus.Approved);
        return await MapAllWithReviews(products);
    }

    public async Task<List<ProductResponse>> GetAllForAdminAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return await MapAllWithReviews(products);
    }

    public async Task<List<ProductResponse>> GetPendingAsync()
    {
        var products = await _productRepository.GetByStatusAsync(ProductStatus.Pending);
        return await MapAllWithReviews(products);
    }

    public async Task<List<ProductResponse>> GetMyProductsAsync(Guid userId)
    {
        var products = await _productRepository.GetByUserIdAsync(userId);
        return await MapAllWithReviews(products);
    }

    public async Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request, Guid userId)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product with id '{id}' not found");

        if (product.UserId != userId)
            throw new UnauthorizedException("You can only update your own products");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.ImageUrl = request.ImageUrl ?? request.ImageUrls?.FirstOrDefault();
        product.ImageUrls = request.ImageUrls != null ? JsonSerializer.Serialize(request.ImageUrls) : null;
        product.Category = request.Category;
        product.Condition = request.Condition;
        product.ContactNote = request.ContactNote;
        product.CategoryId = request.CategoryId;
        product.Status = ProductStatus.Pending;

        await _productRepository.UpdateAsync(product);

        return MapToResponse(product);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product with id '{id}' not found");

        if (product.UserId != userId)
            throw new UnauthorizedException("You can only delete your own products");

        await _productRepository.DeleteAsync(product);
    }

    public async Task<ProductResponse> ApproveAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product with id '{id}' not found");

        product.Status = ProductStatus.Approved;
        await _productRepository.UpdateAsync(product);

        return MapToResponse(product);
    }

    public async Task<ProductResponse> RejectAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product with id '{id}' not found");

        product.Status = ProductStatus.Rejected;
        await _productRepository.UpdateAsync(product);

        return MapToResponse(product);
    }

    private async Task<List<ProductResponse>> MapAllWithReviews(List<Product> products)
    {
        var result = new List<ProductResponse>();
        foreach (var p in products)
        {
            var reviews = await _reviewRepository.GetByProductIdAsync(p.Id);
            result.Add(MapToResponse(p, reviews));
        }
        return result;
    }

    private static ProductResponse MapToResponse(Product p, List<Review>? reviews = null)
    {
        List<string>? imageUrls = null;
        if (!string.IsNullOrEmpty(p.ImageUrls))
        {
            try { imageUrls = JsonSerializer.Deserialize<List<string>>(p.ImageUrls); }
            catch { /* ignore */ }
        }

        var avgRating = reviews?.Count > 0 ? reviews.Average(r => r.Rating) : 0;

        return new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            ImageUrls = imageUrls,
            Category = p.Category,
            CategoryName = p.Category,
            CategoryId = p.CategoryId,
            Condition = p.Condition,
            ContactNote = p.ContactNote,
            UserId = p.UserId,
            SellerId = p.UserId,
            SellerName = p.User?.Username,
            Status = p.Status.ToString(),
            AverageRating = Math.Round(avgRating, 1),
            ReviewCount = reviews?.Count ?? 0,
            CreatedAt = p.CreatedAt
        };
    }
}
