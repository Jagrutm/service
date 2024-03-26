using MassTransit;
using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Domain.EventObservers
{
    public class PublishObserver : IPublishObserver
    {
        public async Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            Console.WriteLine($"IPublishObserver  -- PostPublish : {context.MessageId}");
            await Task.FromResult(0);
        }

        public async Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            Console.WriteLine($"IPublishObserver  -- PrePublish : {context.MessageId}");
            await Task.FromResult(0);
        }

        public async Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            Console.WriteLine($"IPublishObserver  -- PublishFault : {context.Message} : {exception.Message}");
            await Task.FromResult(0);
        }
    }
}
