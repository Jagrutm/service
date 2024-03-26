using EventBus.Messages.Events;
using GreenPipes;
using MassTransit;
using EventBus.Messages.Common;

namespace EventBusConsumer
{
    public class EventOneConsumer : IConsumer<MessageEvent>
    {
        public async Task Consume(ConsumeContext<MessageEvent> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Console.WriteLine(EventBusConstants.GetMessage(context, EventBusConstants.AccountUpdateQueue));
            await Task.CompletedTask;
        }
    }

    public class EventOneConsumerFault : IConsumerFactory<MessageEvent>
    {
        public void Probe(ProbeContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Console.WriteLine(context.ToString());
        }

        public async Task Send<T>(ConsumeContext<T> context, IPipe<ConsumerConsumeContext<MessageEvent, T>> next) where T : class
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Console.WriteLine(context.Message.ToString());
            await Task.CompletedTask;
        }
    }
}
