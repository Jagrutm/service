using System;
using System.Threading.Tasks;

namespace EventBus.Messages.Contracts
{
    public interface IPublisherMq
    {
        Task PublishAsync(Uri uri, object message);

        Task PublishAsync(string queueName, object message);

        Task PublishAsync(string queueName, object message, string exchangeName);
    }
}
