namespace EduCycle.Api.Contracts.Products;

public record CreateProductRequest(
    string Title,
    string Description,
    decimal Price,
    int CategoryId
);
