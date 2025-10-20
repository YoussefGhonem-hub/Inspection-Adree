using Inspection.Application.Wrappers;
using Inspection.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Inspection.Application.Features.InspectionVisits.Commands.UpdateEntityToInspectCommand;
public class UpdateEntityToInspectCommand : IRequest<Response<Guid>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Category { get; set; }
}

public class UpdateEntityToInspectCommandHandler : IRequestHandler<UpdateEntityToInspectCommand, Response<Guid>>
{
    private readonly AppDbContext _context;

    public UpdateEntityToInspectCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Guid>> Handle(UpdateEntityToInspectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.EntityToInspects.FirstOrDefaultAsync(e => e.Id == request.Id);
        if (entity == null)
        {
            return new Response<Guid>(HttpStatusCode.NotFound, "Entity not found.");
        }

        entity.Name = request.Name;
        entity.Address = request.Address;
        entity.Category = request.Category;

        _context.EntityToInspects.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response<Guid>(entity.Id, true, "Entity updated successfully.");
    }
}

