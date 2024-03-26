using EventBus.Messages.Events;
using MassTransit;

namespace AccountService.Application.Features.Consumers.CreateAccount
{
    public class CreateAccountConsumerFault : IConsumerFactory<MessageEvent>
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
