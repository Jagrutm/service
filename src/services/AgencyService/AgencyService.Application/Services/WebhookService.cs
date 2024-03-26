using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Services;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Models.Webhooks;
using AgencyService.Domain.Entities;
using AgencyService.Domain.Enums;
using AutoMapper;
using BuildingBlocks.Core.Extensions;

namespace AgencyService.Application.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly IMapper _mapper;
        private readonly IWebhookRepository _webhookRepository;
        private readonly IWebhookValidator _webhookValidator;

        public WebhookService(
            IMapper mapper,
            IWebhookRepository webhookRepository,
            IWebhookValidator webhookValidator)
        {
            _mapper = mapper;
            _webhookRepository = webhookRepository;
            _webhookValidator = webhookValidator;
        }

        public async Task<List<WebhookResponseDto>> GetWebhooksAsync(Guid agencyId)
        {
            var webhooks = await _webhookRepository.GetWebhooksForAgency(agencyId);
            _webhookValidator.ValidateWebhooksIsNull(webhooks);

            return webhooks
                .Select(_ => new WebhookResponseDto
                {
                    Url = _.Url,
                    WebhookName = EnumExtension.GetEnumNameByValue<WebhookType>(_.Type)
                }).ToList();
        }

        public async Task<WebhookResponseDto> GetWebhookAsync(Guid agencyId, int webhookId)
        {
            var webhook = await _webhookRepository.GetWebhookForAgency(agencyId, webhookId);
            _webhookValidator.ValidateWebhookIsNotNull(webhook);

            var webhookdetail = _mapper.Map<WebhookResponseDto>(webhook);
            webhookdetail.WebhookName = EnumExtension.GetEnumNameByValue<WebhookType>(webhook.Type); //Enum.GetName((WebhookType)_.WebhookId)
            return webhookdetail;
        }

        public async Task CreateWebhookAsync(CreateWebhookDto createWebhookDto)
        {
            var webhook = _mapper.Map<Webhook>(createWebhookDto);
            await _webhookRepository.CreateAsync(webhook);
        }

        public async Task UpdateWebhookAsync(Guid agencyId, int webhookId, UpdateWebhookDto updateWebhookDto)
        {
            var webhook = await GetWebhookIfNotNull(agencyId, webhookId);
            webhook.Url = updateWebhookDto.Url;
            await _webhookRepository.UpdateAsync(webhook);
        }

        public async Task DeleteWebhookAsync(Guid agencyId, int webhookId)
        {
            var webhook = await GetWebhookIfNotNull(agencyId, webhookId);
            await _webhookRepository.DeleteAsync(webhook);
        }

        private async Task<Webhook> GetWebhookIfNotNull(Guid agencyId, int webhookId)
        {
            var webhook = await _webhookRepository.GetWebhookForAgency(agencyId, webhookId);
            _webhookValidator.ValidateWebhookIsNotNull(webhook);
            return webhook;
        }
    }
}
