using EduCycle.Contracts.Transactions;
using FluentValidation;

namespace EduCycle.Application.Validators;

public class UpdateTransactionStatusRequestValidator : AbstractValidator<UpdateTransactionStatusRequest>
{
    private static readonly string[] ValidStatuses = ["Pending", "Accepted", "Completed", "Cancelled"];

    public UpdateTransactionStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage("Status must be one of: Pending, Accepted, Completed, Cancelled.");
    }
}
