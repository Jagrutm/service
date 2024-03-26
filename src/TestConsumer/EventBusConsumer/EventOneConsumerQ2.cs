using EventBus.Messages.Common;
using EventBus.Messages.Events;
using GreenPipes;
using MassTransit;

namespace EventBusConsumer
{
    public class EventOneConsumerQ2 : IConsumer<MessageEvent>
    {
        public async Task Consume(ConsumeContext<MessageEvent> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Console.WriteLine(EventBusConstants.GetMessage(context, EventBusConstants.AccountCreateQueue));
            await Task.CompletedTask;
        }
    }

    public class EventOneConsumerQ2Fault : IConsumerFactory<MessageEvent>
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
