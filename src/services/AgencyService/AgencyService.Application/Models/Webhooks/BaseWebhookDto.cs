namespace AgencyService.Application.Models.Webhooks
{
    public abstract class BaseWebhookDto
    {
        public int AgencyId { get; set; }

        public int WebhookId { get; set; }

        public string Url { get; set; }
    }
}
