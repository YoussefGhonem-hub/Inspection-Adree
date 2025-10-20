using Inspection.Application.Features.Inspectors.Queries.GetAllInspectorsQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inspection.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InspectorController : ControllerBase
{
    private readonly IMediator _mediator;

    public InspectorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInspectors()
    {
        var result = await _mediator.Send(new GetAllInspectorsQuery());
        return result.Succeeded ? Ok(result.Data) : BadRequest(result);
    }
}
