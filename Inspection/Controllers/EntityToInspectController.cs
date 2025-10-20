using Inspection.Application.Features.InspectionVisits.Commands.AssignInspectionVisitCommand;
using Inspection.Application.Features.InspectionVisits.Commands.CreateInspectionVisitCommand;
using Inspection.Application.Features.InspectionVisits.Commands.DeleteEntityToInspectCommand;
using Inspection.Application.Features.InspectionVisits.Commands.UpdateEntityToInspectCommand;
using Inspection.Application.Features.InspectionVisits.Queries.DashboardQuery;
using Inspection.Application.Features.InspectionVisits.Queries.GetInspectionVisitsByFiltersQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inspection.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntityToInspectController : ControllerBase
{
    private readonly IMediator _mediator;

    public EntityToInspectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetInspectionVisitsByFiltersQuery query)
    {
        var result = await _mediator.Send(query);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetById()
    {
        var result = await _mediator.Send(new DashboardQuery());
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEntityToInspectCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? CreatedAtAction(nameof(GetById), new { id = result.Data }, result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEntityToInspectCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpPut("assign")]
    public async Task<IActionResult> Assign([FromBody] AssignInspectionVisitCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteEntityToInspectCommand { Id = id });
        return result.Succeeded ? Ok() : NotFound(result);
    }
}