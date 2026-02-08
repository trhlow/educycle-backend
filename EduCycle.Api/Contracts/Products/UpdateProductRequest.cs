namespace EduCycle.Api.Contracts.Products;

public record UpdateProductRequest(
    string Title,
    string Description,
    decimal Price,
    int CategoryId
);
