using AutoMapper;
using Inspection.Application.Wrappers;
using Inspection.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Inspection.Application.Features.Inspectors.Queries.GetAllInspectorsQuery;
public class GetAllInspectorsQuery : IRequest<Response<IEnumerable<InspectorDto>>>
{
    // You can add filters or pagination properties if needed in the future
}

public class GetAllInspectorsQueryHandler : IRequestHandler<GetAllInspectorsQuery, Response<IEnumerable<InspectorDto>>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllInspectorsQueryHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Response<IEnumerable<InspectorDto>>> Handle(GetAllInspectorsQuery request, CancellationToken cancellationToken)
    {
        var inspectors = await _context.Inspectors.AsNoTracking().ToListAsync(cancellationToken);

        if (inspectors == null || !inspectors.Any())
        {
            return new Response<IEnumerable<InspectorDto>>(HttpStatusCode.NotFound, "No inspectors found.");
        }

        var inspectorDtos = _mapper.Map<IEnumerable<InspectorDto>>(inspectors);

        return new Response<IEnumerable<InspectorDto>>(inspectorDtos, true, "Inspectors retrieved successfully.");
    }
}
