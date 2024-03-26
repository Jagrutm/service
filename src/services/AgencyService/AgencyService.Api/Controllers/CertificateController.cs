using AgencyService.Application.Features.Certificate.Commands;
using AgencyService.Application.Features.Certificate.Queries;
using BuildingBlocks.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Controllers
{
    [Route("agencies")]
    public class CertificateController : BaseController
    {
        private readonly IMediator _mediator;

        public CertificateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("{agencyId}/[controller]")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCertificate(
            [FromRoute] Guid agencyId,
            [FromBody] CreateCertificateCommand createCertificateCommand)
        {
            createCertificateCommand.AgencyId = agencyId;
            var result = await _mediator.Send(createCertificateCommand);
            return Created(string.Empty, result);
        }

        [HttpGet]
        [Route("{agencyId}/[controller]/{certificateId}")]
        [ProducesResponseType(typeof(CertificateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCertificateById(
            [FromRoute] Guid agencyId,
            [FromRoute] int certificateId)
        {
            var query = new GetCertificateByIdQuery
            {
                AgencyId = agencyId,
                CertificateId = certificateId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("{agencyId}/[controller]")]
        [ProducesResponseType(typeof(List<CertificateResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCertificatesForAgency([FromRoute] Guid agencyId)
        {
            var query = new GetCertificatesQuery
            {
                AgencyId = agencyId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("{agencyId}/[controller]/{certificateId}/verify")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyCertificate(
            [FromRoute] Guid agencyId,
            [FromRoute] int certificateId)
        {
            var query = new VerifyCertificateQuery
            {
                AgencyId = agencyId,
                CertificateId = certificateId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        [Route("{agencyId}/[controller]/{certificateId}/activate")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ActivateCertificate(
            [FromRoute] Guid agencyId,
            [FromRoute] int certificateId)
        {
            var command = new ActivateCertificateCommand
            {
                AgencyId = agencyId,
                CertificateId = certificateId
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete]
        [Route("{agencyId}/[controller]/{certificateId}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCertificate(
            [FromRoute] Guid agencyId,
            [FromRoute] int certificateId)
        {
            var command = new DeleteCertificateCommand
            {
                AgencyId = agencyId,
                CertificateId = certificateId
            };

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
