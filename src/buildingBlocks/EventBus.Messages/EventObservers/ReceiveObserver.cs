using MassTransit;
using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Domain.EventObservers
{
    public class ReceiveObserver : IReceiveObserver
    {
        public async Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception) where T : class
        {
            Console.WriteLine($"IReceiveObserver -- ConsumeFault {context.Message} : {duration.TotalSeconds} : {consumerType.ToLower()} : {exception.Message}");
            await context.ConsumeCompleted;
        }

        public async Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType) where T : class
        {
            Console.WriteLine($"IReceiveObserver -- PostConsume {context.Message} : {duration.TotalSeconds} : {consumerType.ToLower()}");
            await context.ConsumeCompleted;
        }

        public async Task PostReceive(ReceiveContext context)
        {
            Console.WriteLine($"IReceiveObserver -- {context.GetMessageId()}");
            await context.ReceiveCompleted;
        }

        public async Task PreReceive(ReceiveContext context)
        {
            Console.WriteLine($"IReceiveObserver -- PreReceive : {context.GetMessageId()}");
            await context.ReceiveCompleted;
        }

        public async Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            Console.WriteLine($"IReceiveObserver -- ReceiveFault : {context.GetMessageId()} : {exception.Message}");
            await context.ReceiveCompleted;
        }
    }
}
