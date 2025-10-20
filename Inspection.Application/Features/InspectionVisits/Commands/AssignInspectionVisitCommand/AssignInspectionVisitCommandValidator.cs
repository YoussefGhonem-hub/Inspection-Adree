namespace Inspection.Application.Features.InspectionVisits.Commands.AssignInspectionVisitCommand;
using FluentValidation;
using Inspection.Domain.Enums;

public class AssignInspectionVisitCommandValidator : AbstractValidator<AssignInspectionVisitCommand>
{
    public AssignInspectionVisitCommandValidator()
    {
        RuleFor(x => x.EntityToInspectId)
            .NotEmpty().WithMessage("Entity to inspect is required.");

        RuleFor(x => x.InspectorId)
            .NotEmpty().WithMessage("Inspector is required.");

        RuleFor(x => x.ScheduledAt)
            .GreaterThan(DateTime.Now).WithMessage("Scheduled date must be in the future.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value.")
            .Must(BeValidStatusTransition).WithMessage("Invalid status transition.");

        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notes are required.")
            .MaximumLength(500).WithMessage("Notes should not exceed 500 characters.");
    }

    // Custom rule to ensure valid status transitions
    private bool BeValidStatusTransition(AssignInspectionVisitCommand command, VisitStatus status)
    {
        // If the current status is `Planned`, we allow it as an initial state (first creation)
        if (status == VisitStatus.Planned)
        {
            return true; // Allow `Planned` as the initial status
        }

        // If status is InProgress, we can transition to Completed or Cancelled
        if (status == VisitStatus.InProgress)
        {
            return command.Status == VisitStatus.Completed || command.Status == VisitStatus.Cancelled;
        }

        // If status is Completed or Cancelled, it should not change anymore
        if (status == VisitStatus.Completed || status == VisitStatus.Cancelled)
        {
            return false;
        }

        return false; // If no valid transition is matched
    }
}
