using FluentValidation;
using Inspection.Domain.Enums;

namespace Inspection.Application.Features.InspectionVisits.Commands.UpdateInspectionVisitStatusCommand;
public class UpdateInspectionVisitStatusCommandValidator : AbstractValidator<UpdateInspectionVisitStatusCommand>
{
    public UpdateInspectionVisitStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Inspection Visit Id is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value.");

        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(0).When(x => x.Status == VisitStatus.Completed)
            .WithMessage("Score is required for completed visits.");

        RuleForEach(x => x.Violations)
            .ChildRules(violations =>
            {
                violations.RuleFor(v => v.Code)
                          .NotEmpty().WithMessage("Violation code is required.");
                violations.RuleFor(v => v.Description)
                          .NotEmpty().WithMessage("Violation description is required.");
            });
    }
}
