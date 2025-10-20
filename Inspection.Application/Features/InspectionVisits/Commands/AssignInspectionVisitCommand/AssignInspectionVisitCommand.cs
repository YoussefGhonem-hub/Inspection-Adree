using Inspection.Application.Wrappers;
using Inspection.Domain.Entities;
using Inspection.Domain.Enums;
using Inspection.Infrastructure.Persistence;
using MediatR;

namespace Inspection.Application.Features.InspectionVisits.Commands.AssignInspectionVisitCommand;
public class AssignInspectionVisitCommand : IRequest<Response<Guid>>
{
    public Guid EntityToInspectId { get; set; }
    public Guid InspectorId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public VisitStatus Status { get; set; }
    public string Notes { get; set; }
}

public class AssignInspectionVisitCommandHandler : IRequestHandler<AssignInspectionVisitCommand, Response<Guid>>
{
    private readonly AppDbContext _context;

    public AssignInspectionVisitCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Guid>> Handle(AssignInspectionVisitCommand request, CancellationToken cancellationToken)
    {
        var inspectionVisit = new InspectionVisit
        {
            EntityToInspectId = request.EntityToInspectId,
            InspectorId = request.InspectorId,
            ScheduledAt = request.ScheduledAt,
            Status = request.Status,
            Notes = request.Notes
        };

        _context.InspectionVisits.Add(inspectionVisit);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response<Guid>(inspectionVisit.Id, true, "Inspection visit assigned successfully.");
    }
}
