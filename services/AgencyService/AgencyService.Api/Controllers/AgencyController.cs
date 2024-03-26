using AgencyService.Application.Features.Agency.Commands.CreateAgency;
using AgencyService.Application.Features.Agency.Commands.DeleteAgency;
using AgencyService.Application.Features.Agency.Commands.UpdateAgency;
using AgencyService.Application.Features.Agency.Queries.GetAgency;
using AgencyService.Application.Features.Agency.Queries.GetAgencyList;
using BuildingBlocks.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Controllers;

public class AgencyController : BaseController
{
    private readonly IMediator _mediator;

    public AgencyController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [ProducesResponseType(typeof(AgencyResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateAgency([FromBody] CreateAgencyCommand command)
    {
        var result = await _mediator.Send(command);
        return Created(string.Empty, result);
    }

    [HttpPut]
    [Route("{agencyId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAgency(
        [FromRoute] Guid agencyId,
        [FromBody] UpdateAgencyCommand command)
    {
        command.AgencyId = agencyId;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    [Route("{agencyId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAgency([FromRoute] Guid agencyId)
    {
        var command = new DeleteAgencyCommand
        { 
            AgencyId = agencyId
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet]   
    [ProducesResponseType(typeof(List<AgencyResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAgencies()
    {
        var query = new GetAgenciesQuery();
        var agencies = await _mediator.Send(query);
        return Ok(agencies);
    }

    [HttpGet]
    [Route("{agencyId}")]
    [ProducesResponseType(typeof(AgencyResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAgencyById([FromRoute] Guid agencyId)
    {
        var query = new GetAgencyQuery
        {
            AgencyId = agencyId
        };

        var agency = await _mediator.Send(query);
        return Ok(agency);
    }
}
