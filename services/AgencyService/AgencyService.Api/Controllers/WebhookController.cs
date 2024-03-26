using AgencyService.Application.Contracts.Services;
using AgencyService.Application.Models.Webhooks;
using BuildingBlocks.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Api.Controllers
{
    public class WebhookController : BaseController
    {
        private readonly IWebhookService _webhookService;

        public WebhookController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateWebhook([FromBody] CreateWebhookDto createWebhookDto)
        {
            await _webhookService.CreateWebhookAsync(createWebhookDto);
            return Created(string.Empty, null);
        }

        [HttpPut]
        [Route("agency/{agencyId}/webhook/{webhookId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateWebhook(
            Guid agencyId, 
            int webhookId, 
            [FromBody] UpdateWebhookDto updateWebhookDto)
        {
            await _webhookService.UpdateWebhookAsync(agencyId, webhookId, updateWebhookDto);
            return NoContent();
        }

        [HttpDelete]
        [Route("agency/{agencyId}/webhook/{webhookId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteWebhook(Guid agencyId, int webhookId)
        {
            await _webhookService.DeleteWebhookAsync(agencyId, webhookId);
            return NoContent();
        }

        [HttpGet]
        [Route("{agencyId}")]
        [ProducesResponseType(typeof(List<WebhookResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWebhooks(Guid agencyId)
        {
            return Ok(await _webhookService.GetWebhooksAsync(agencyId));
        }

        [HttpGet]
        [Route("agency/{agencyId}/webhook/{webhookId}")]
        [ProducesResponseType(typeof(WebhookResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWebhook(Guid agencyId, int webhookId)
        {
            return Ok(await _webhookService.GetWebhookAsync(agencyId, webhookId));
        }
    }
}
