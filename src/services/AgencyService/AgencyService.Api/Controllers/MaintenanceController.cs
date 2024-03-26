using AgencyService.Application.Contracts.Services;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Models.Maintenances;
using BuildingBlocks.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Controllers
{
    [Route("agencies")]
    public class MaintenanceController : BaseController
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IMaintenanceValidator _maintenanceValidator;

        public MaintenanceController(
            IMaintenanceService maintenanceService,
            IMaintenanceValidator maintenanceValidator)
        {
            _maintenanceService = maintenanceService;
            _maintenanceValidator = maintenanceValidator;
        }

        [HttpPost]
        [Route("{agencyId}/[controller]")]
        [ProducesResponseType(typeof(CreateMaintenanceDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateMaintenance(
            [FromRoute] Guid agencyId,
            [FromBody] CreateMaintenanceDto maintenanceDto)
        {
            await _maintenanceService.CreateMaintenanceForAgencyAsync(agencyId, maintenanceDto);
            return Created(string.Empty, maintenanceDto);
        }

        [HttpGet]
        [Route("{agencyId}/[controller]")]
        [ProducesResponseType(typeof(List<MaintenanceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaintenances([FromRoute] Guid agencyId)
        {
            var maintenanceDtos = await _maintenanceService.GetMaintenancesAsync(agencyId);
            return Ok(maintenanceDtos);
        }

        [HttpGet]
        [Route("{agencyId}/[controller]/{maintenanceId}")]
        [ProducesResponseType(typeof(MaintenanceDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaintenanceDetailById(
            [FromRoute] Guid agencyId,
            [FromRoute] int maintenanceId)
        {
            var maintenanceDetails = await _maintenanceService.GetMaintenanceDetailsAsync(agencyId, maintenanceId);
            return Ok(maintenanceDetails);
        }

        [HttpPut]
        [Route("{agencyId}/[controller]/{maintenanceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateMaintenanceDetails(
            [FromRoute] Guid agencyId,
            [FromRoute] int maintenanceId, 
            [FromBody] UpdateMaintenanceDto maintenanceDto)
        {
            await _maintenanceService.UpdateMaintenanceForAgencyAsync(agencyId, maintenanceId, maintenanceDto);
            return NoContent();
        }

        [HttpDelete]
        [Route("{agencyId}/[controller]/{maintenanceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMaintenanceDetails(
            [FromRoute] Guid agencyId, 
            [FromRoute] int maintenanceId)
        {
            await _maintenanceService.DeleteMaintenanceForAgencyAsync(agencyId, maintenanceId);
            return NoContent();
        }
    }
}
