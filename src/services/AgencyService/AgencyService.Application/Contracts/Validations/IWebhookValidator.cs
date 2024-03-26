using AgencyService.Domain.Entities;

namespace AgencyService.Application.Contracts.Validations
{
    public interface IWebhookValidator
    {
        Task ValidateWebhookWithIdExists(Guid agencyId, int webhookId);

        void ValidateWebhookIsNotNull(Webhook webhook);

        void ValidateWebhooksIsNull(List<Webhook> webhooks);
    }
}
