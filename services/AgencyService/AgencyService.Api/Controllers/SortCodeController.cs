using AgencyService.Application.Contracts.Services;
using AgencyService.Application.Models.SortCodes;
using BuildingBlocks.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Controllers
{
    [Route("agencies")]
    public class SortCodeController : BaseController
    {
        private readonly ISortCodeService _sortCodeService; 

        public SortCodeController(ISortCodeService sortCodeService)
        {
            _sortCodeService = sortCodeService;
        }

        [HttpPost]
        [Route("{agencyId}/[Controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSortCode(
            [FromRoute] Guid agencyId, 
            [FromBody] CreateSortCodeDto sortCodeToCreate)
        { 
            await _sortCodeService.CreateSortCodeForAgencyAsync(agencyId, sortCodeToCreate);
            return Created(string.Empty, sortCodeToCreate);
        }

        [HttpGet]
        [Route("{agencyId}/[Controller]")]
        [ProducesResponseType(typeof(List<SortCodeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSortCodesForAgency([FromRoute] Guid agencyId)
        { 
            var sortCodesForAgency = await _sortCodeService.GetSortcodesForAgencyAsync(agencyId);
            return Ok(sortCodesForAgency);
        }

        [HttpGet]
        [Route("[Controller]")]
        [ProducesResponseType(typeof(List<AgencyIdSortCodeTupleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSortCodeForAllAgencies()
        { 
            var agencyIdSortCodeTupleDtos = await _sortCodeService.GetSortCodeForAllAgenciesAsync();
            return Ok(agencyIdSortCodeTupleDtos);
        }

        [HttpGet]
        [Route("{agencyId}/[Controller]/{sortCode}/verify")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifySortCodeForAgency(
            [FromRoute] Guid agencyId, 
            [FromRoute] string sortCode)
        { 
            var isVerified = await _sortCodeService.VerifySortCodeForAgencyAsync(agencyId, sortCode);
            return Ok(isVerified);
        }
    }
}
