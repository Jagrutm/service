using AgencyService.Domain.Entities;
using BuildingBlocks.Application.Contracts.Persistence;

namespace AgencyService.Application.Contracts.Persistence
{
    public interface IWebhookRepository : IEFRepository<Webhook, int>
    {
        Task<List<Webhook>> GetWebhooksForAgency(Guid agencyId);

        Task<Webhook> GetWebhookForAgency(Guid agencyId, int webhookId);
    }
}
