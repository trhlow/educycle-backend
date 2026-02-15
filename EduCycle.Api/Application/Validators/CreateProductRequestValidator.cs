using EduCycle.Contracts.Products;
using FluentValidation;

namespace EduCycle.Application.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(5).WithMessage("Product name must be at least 5 characters.")
            .MaximumLength(150).WithMessage("Product name must not exceed 150 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.")
            .LessThanOrEqualTo(10000000).WithMessage("Price must not exceed 10,000,000.");

        RuleFor(x => x.Description)
            .MinimumLength(20).WithMessage("Description must be at least 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
