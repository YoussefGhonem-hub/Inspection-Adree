using AutoMapper;
using Inspection.Application.Wrappers;
using Inspection.Domain.Enums;
using Inspection.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspection.Application.Features.InspectionVisits.Queries.GetInspectionVisitsByFiltersQuery;
public class GetInspectionVisitsByFiltersQuery : IRequest<Response<IEnumerable<InspectionVisitDto>>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public VisitStatus? Status { get; set; }
    public Guid? InspectorId { get; set; }
    public string? Category { get; set; }
}

public class GetInspectionVisitsByFiltersQueryHandler : IRequestHandler<GetInspectionVisitsByFiltersQuery, Response<IEnumerable<InspectionVisitDto>>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetInspectionVisitsByFiltersQueryHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Response<IEnumerable<InspectionVisitDto>>> Handle(GetInspectionVisitsByFiltersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.InspectionVisits
                     .Include(iv => iv.Inspector)
                     .Include(iv => iv.EntityToInspect)
                     .AsQueryable();

        query = query.ApplyFilters(request);

        var visits = await query.ToListAsync(cancellationToken);

        return new Response<IEnumerable<InspectionVisitDto>>(_mapper.Map<IEnumerable<InspectionVisitDto>>(visits), true, "Inspection visits retrieved successfully.");
    }
}
