using AgencyService.Application.Models.Webhooks;

namespace AgencyService.Application.Contracts.Services
{
    public interface IWebhookService
    {
        Task<List<WebhookResponseDto>> GetWebhooksAsync(Guid agencyId);

        Task<WebhookResponseDto> GetWebhookAsync(Guid agencyId, int webhookId);

        Task CreateWebhookAsync(CreateWebhookDto createWebhookDto);

        Task UpdateWebhookAsync(Guid agencyId, int webhookId, UpdateWebhookDto updateWebhookDto);

        Task DeleteWebhookAsync(Guid agencyId, int webhookId);
    }
}
