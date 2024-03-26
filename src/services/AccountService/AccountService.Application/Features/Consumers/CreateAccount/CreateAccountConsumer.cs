using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using System.Diagnostics;

namespace AccountService.Application.Features.Consumers.CreateAccount
{
    public class CreateAccountConsumer : IConsumer<MessageEvent>
    {
        public async Task Consume(ConsumeContext<MessageEvent> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Console.WriteLine(EventBusConstants.GetMessage(context, EventBusConstants.AccountCreateQueue));
            Debug.WriteLine(EventBusConstants.GetMessage(context, EventBusConstants.AccountCreateQueue));

            //do-something with context.Message.Message

            await Task.CompletedTask;
        }
    }
}
