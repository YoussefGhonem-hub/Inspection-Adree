namespace Inspection.Application.Features.InspectionVisits.Commands.CreateInspectionVisitCommand;
using FluentValidation;

public class CreateEntityToInspectCommandValidator : AbstractValidator<CreateEntityToInspectCommand>
{
    public CreateEntityToInspectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(250).WithMessage("Address cannot exceed 250 characters.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(50).WithMessage("Category cannot exceed 50 characters.");
    }
}
