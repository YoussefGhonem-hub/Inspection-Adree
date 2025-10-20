using Inspection.Application.Wrappers;
using Inspection.Domain.Entities;
using Inspection.Infrastructure.Persistence;
using MediatR;

namespace Inspection.Application.Features.InspectionVisits.Commands.CreateInspectionVisitCommand;
public class CreateEntityToInspectCommand : IRequest<Response<Guid>>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Category { get; set; }
}

public class CreateEntityToInspectCommandHandler : IRequestHandler<CreateEntityToInspectCommand, Response<Guid>>
{
    private readonly AppDbContext _context;

    public CreateEntityToInspectCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Guid>> Handle(CreateEntityToInspectCommand request, CancellationToken cancellationToken)
    {
        var entity = new EntityToInspect
        {
            Name = request.Name,
            Address = request.Address,
            Category = request.Category
        };

        _context.EntityToInspects.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response<Guid>(entity.Id, true, "Entity created successfully.");
    }
}
