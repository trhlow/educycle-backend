using EduCycle.Application.Services;
using EduCycle.Common.Exceptions;
using EduCycle.Contracts.Products;
using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Repositories;
using Moq;

namespace EduCycle.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _sut = new ProductService(_repoMock.Object);
    }

    // ===== CREATE =====

    [Fact]
    public async Task CreateAsync_ShouldReturnProduct()
    {
        var userId = Guid.NewGuid();
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Description = "A test product",
            Price = 100.50m
        };

        var result = await _sut.CreateAsync(request, userId);

        Assert.NotNull(result);
        Assert.Equal("Test Product", result.Name);
        Assert.Equal("A test product", result.Description);
        Assert.Equal(100.50m, result.Price);
        Assert.Equal(userId, result.UserId);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    // ===== GET BY ID =====

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
    {
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Existing Product",
            Description = "Desc",
            Price = 50m,
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);

        var result = await _sut.GetByIdAsync(productId);

        Assert.Equal(productId, result.Id);
        Assert.Equal("Existing Product", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
    {
        var productId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetByIdAsync(productId));
    }

    // ===== GET ALL =====

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "P1", Price = 10, UserId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "P2", Price = 20, UserId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow }
        };

        _repoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(products);

        var result = await _sut.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmpty_WhenNoProducts()
    {
        _repoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync([]);

        var result = await _sut.GetAllAsync();

        Assert.Empty(result);
    }

    // ===== UPDATE =====

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_WhenOwner()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Old Name",
            Price = 10,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);

        var request = new UpdateProductRequest
        {
            Name = "New Name",
            Description = "Updated desc",
            Price = 99.99m
        };

        var result = await _sut.UpdateAsync(productId, request, userId);

        Assert.Equal("New Name", result.Name);
        Assert.Equal(99.99m, result.Price);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNotOwner()
    {
        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Product",
            Price = 10,
            UserId = ownerId,
            CreatedAt = DateTime.UtcNow
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);

        var request = new UpdateProductRequest
        {
            Name = "Hacked",
            Price = 0
        };

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => _sut.UpdateAsync(productId, request, otherUserId));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNotFound()
    {
        var productId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync((Product?)null);

        var request = new UpdateProductRequest { Name = "X", Price = 1 };

        await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.UpdateAsync(productId, request, Guid.NewGuid()));
    }

    // ===== DELETE =====

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenOwner()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Product",
            Price = 10,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);

        await _sut.DeleteAsync(productId, userId);

        _repoMock.Verify(r => r.DeleteAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenNotOwner()
    {
        var ownerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Product",
            Price = 10,
            UserId = ownerId,
            CreatedAt = DateTime.UtcNow
        };

        _repoMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => _sut.DeleteAsync(productId, Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenNotFound()
    {
        var productId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.DeleteAsync(productId, Guid.NewGuid()));
    }
}
