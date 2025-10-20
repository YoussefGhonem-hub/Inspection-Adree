using Inspection.Application.Wrappers;
using Inspection.Domain.Enums;
using Inspection.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspection.Application.Features.InspectionVisits.Queries.DashboardQuery;
public class DashboardQuery : IRequest<Response<DashboardDto>>
{
}

public class DashboardQueryHandler : IRequestHandler<DashboardQuery, Response<DashboardDto>>
{
    private readonly AppDbContext _context;
    public DashboardQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<DashboardDto>> Handle(DashboardQuery request, CancellationToken cancellationToken)
    {
        var plannedCount = await _context.InspectionVisits.CountAsync(iv => iv.Status == VisitStatus.Planned, cancellationToken);
        var inProgressCount = await _context.InspectionVisits.CountAsync(iv => iv.Status == VisitStatus.InProgress, cancellationToken);
        var completedCount = await _context.InspectionVisits.CountAsync(iv => iv.Status == VisitStatus.Completed, cancellationToken);
        var cancelledCount = await _context.InspectionVisits.CountAsync(iv => iv.Status == VisitStatus.Cancelled, cancellationToken);

        var averageScore = completedCount > 0 ? await _context.InspectionVisits.Where(iv => iv.Status == VisitStatus.Completed).AverageAsync(iv => iv.Score, cancellationToken) : 0;

        var dashboard = new DashboardDto
        {
            PlannedCount = plannedCount,
            InProgressCount = inProgressCount,
            CompletedCount = completedCount,
            CancelledCount = cancelledCount,
            AverageScore = averageScore
        };

        return new Response<DashboardDto>(dashboard, true, "Dashboard data retrieved successfully.");
    }
}
