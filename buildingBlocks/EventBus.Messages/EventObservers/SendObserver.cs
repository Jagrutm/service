using MassTransit;
using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Domain.EventObservers
{
    public class SendObserver : ISendObserver
    {
        public async Task PostSend<T>(SendContext<T> context) where T : class
        {
            Console.WriteLine($"ISendObserver -- PostSend : {context.MessageId}");
            await Task.FromResult(0);
        }

        public async Task PreSend<T>(SendContext<T> context) where T : class
        {
            Console.WriteLine($"ISendObserver -- PreSend : {context.MessageId}");
            await Task.FromResult(0);
        }

        public async Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            Console.WriteLine($"ISendObserver -- SendFault : {context.MessageId} : {exception.Message}");
            await Task.FromResult(0);
        }
    }
}
