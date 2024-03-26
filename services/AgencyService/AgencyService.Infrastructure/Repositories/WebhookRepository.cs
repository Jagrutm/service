using AgencyService.Application.Contracts.Persistence;
using AgencyService.Domain.Entities;
using AgencyService.Infrastructure.Persistence;
using BuildingBlocks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AgencyService.Infrastructure.Repositories
{
    public class WebhookRepository : EFRepositoryBase<Webhook, int>, IWebhookRepository
    {

        public WebhookRepository(AgencyDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Webhook> GetWebhookForAgency(Guid agencyId, int webhookId)
        {
            var result = await (from webhook in _dbContext.Set<Webhook>()
                          join agency in _dbContext.Set<Agency>()
                          on webhook.AgencyId equals agency.Id
                          where agency.UId == agencyId && webhook.Type == webhookId
                          select new Webhook
                          {
                              Id = webhook.Id,
                              AgencyId = agency.Id,
                              Type = webhook.Type,
                              Url = webhook.Url
                          }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<List<Webhook>> GetWebhooksForAgency(Guid agencyId)
        {
            var result = await (from webhook in _dbContext.Set<Webhook>()
                                join agency in _dbContext.Set<Agency>()
                                on webhook.AgencyId equals agency.Id
                                where agency.UId == agencyId
                                select new Webhook
                                {
                                    Id = webhook.Id,
                                    AgencyId = agency.Id,
                                    Type = webhook.Type,
                                    Url = webhook.Url
                                }).ToListAsync();

            return result;
        }

        protected override IQueryable<Webhook> IncludeChildEntitiesIn(IQueryable<Webhook> queryable)
        {
            return queryable;
        }
    }
}
