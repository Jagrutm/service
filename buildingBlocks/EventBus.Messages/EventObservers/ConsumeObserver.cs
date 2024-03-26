using MassTransit;
using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Domain.EventObservers
{
    public class ConsumeObserver : IConsumeObserver
    {
        public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            Console.WriteLine($"IConsumeObserver -- ConsumeFault : {context.Message} : {exception.Message} ");
            await context.ConsumeCompleted;
        }

        public async Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            Console.WriteLine($"IConsumeObserver -- PostConsume : {context.Message} ");
            await context.ConsumeCompleted;
        }

        public async Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            Console.WriteLine($"IConsumeObserver -- PreConsume : {context.Message} ");
            await context.ConsumeCompleted;
        }
    }
}
