using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Validations.BusinessValidators
{
    public class WebhookValidator : IWebhookValidator
    {
        private readonly ILogger<WebhookValidator> _logger;
        private readonly IWebhookRepository _webhookRepository;

        public WebhookValidator(ILogger<WebhookValidator> logger, IWebhookRepository webhookRepository)
        {
            _logger = logger;
            _webhookRepository = webhookRepository;
        }

        public void ValidateWebhookIsNotNull(Webhook webhook)
        {
            if(webhook == null)
            {
                _logger.LogInformation("Webhook not found");
                throw new Exception("Webhook not found");
            }

            _logger.LogDebug("Validated webhook with id {WebhookId} exists for agency with id {AgencyId}", webhook?.Type + " & " + webhook?.AgencyId);
        }

        public void ValidateWebhooksIsNull(List<Webhook> webhooks)
        {
            if (webhooks == null)
            {
                _logger.LogInformation("Webhooks not found");
                throw new Exception("Webhooks not found");
            }
        }

        public async Task ValidateWebhookWithIdExists(Guid agencyId, int webhookId)
        {
            var webhook = await _webhookRepository.GetWebhookForAgency(agencyId, webhookId);
            ValidateWebhookIsNotNull(webhook);
        }
    }
}
