using Inspection.Application.Wrappers;
using Inspection.Domain.Entities;
using Inspection.Domain.Enums;
using Inspection.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Inspection.Application.Features.InspectionVisits.Commands.UpdateInspectionVisitStatusCommand;
public class UpdateInspectionVisitStatusCommand : IRequest<Response<Guid>>
{
    public Guid Id { get; set; }
    public VisitStatus Status { get; set; }
    public int? Score { get; set; }
    public List<Violation> Violations { get; set; }
}

public class UpdateInspectionVisitStatusCommandHandler : IRequestHandler<UpdateInspectionVisitStatusCommand, Response<Guid>>
{
    private readonly AppDbContext _context;

    public UpdateInspectionVisitStatusCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Guid>> Handle(UpdateInspectionVisitStatusCommand request, CancellationToken cancellationToken)
    {
        var visit = await _context.InspectionVisits.Include(iv => iv.Violations).FirstOrDefaultAsync(iv => iv.Id == request.Id);
        if (visit == null)
        {
            return new Response<Guid>(HttpStatusCode.NotFound, "Inspection visit not found.");
        }

        visit.Status = request.Status;
        if (request.Status == VisitStatus.Completed)
        {
            visit.Score = request.Score ?? 0;
            visit.Violations = request.Violations;
        }

        _context.InspectionVisits.Update(visit);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response<Guid>(visit.Id, true, "Inspection visit status updated.");
    }
}
