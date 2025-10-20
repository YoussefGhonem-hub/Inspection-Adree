using Inspection.Application.Wrappers;
using Inspection.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Inspection.Application.Features.InspectionVisits.Commands.DeleteEntityToInspectCommand;
public class DeleteEntityToInspectCommand : IRequest<Response<Guid>>
{
    public Guid Id { get; set; }
}

public class DeleteEntityToInspectCommandHandler : IRequestHandler<DeleteEntityToInspectCommand, Response<Guid>>
{
    private readonly AppDbContext _context;

    public DeleteEntityToInspectCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Guid>> Handle(DeleteEntityToInspectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.EntityToInspects.FirstOrDefaultAsync(e => e.Id == request.Id);
        if (entity == null)
        {
            return new Response<Guid>(HttpStatusCode.NotFound, "Entity not found.");
        }

        _context.EntityToInspects.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response<Guid>(entity.Id, true, "Entity deleted successfully.");
    }
}
