using MassTransit;
using System;
using System.Threading.Tasks;

namespace EventBus.Messages.Contracts
{
    public class MassTransitRabbitMq : IPublisherMq
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IBus _bus;

        public MassTransitRabbitMq(IPublishEndpoint publishEndpoint, IBus bus)
        {
            _publishEndpoint = publishEndpoint;
            _bus = bus;
        }

        public async Task PublishAsync(Uri uri, object message)
        {
            await SendAsync(uri, message);
        }

        public async Task PublishAsync(string queueName, object message)
        {
            await SendAsync(GetQueueUrl(queueName), message);
        }

        public async Task PublishAsync(string queueName, object message, string exchangeName)
        {
            await SendAsync(GetQueueUrl(queueName, exchangeName), message);
        }

        #region Private Methods
        private async Task SendAsync(Uri uri, object message)
        {
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(message);
        }

        private static Uri GetQueueUrl(string queueName, string exchangeName = default, bool bind = true)
        {
            if (string.IsNullOrEmpty(exchangeName)) exchangeName = queueName;
            return new Uri($"queue:{queueName}");
        }
        #endregion
    }
}
