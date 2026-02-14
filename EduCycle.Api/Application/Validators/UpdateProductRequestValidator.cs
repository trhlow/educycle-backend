using EduCycle.Contracts.Products;
using FluentValidation;

namespace EduCycle.Application.Validators;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be greater than 0.")
            .When(x => x.CategoryId.HasValue);
    }
}
