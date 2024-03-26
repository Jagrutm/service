using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using System.Diagnostics;

namespace AccountService.Application.Features.Consumers.DeleteAccount
{
    public class DeleteAccountConsumer : IConsumer<MessageEvent>
    {
        public async Task Consume(ConsumeContext<MessageEvent> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Console.WriteLine(EventBusConstants.GetMessage(context, EventBusConstants.AccountDeleteQueue));
            Debug.WriteLine(EventBusConstants.GetMessage(context, EventBusConstants.AccountDeleteQueue));

            //do-something with context.Message.Message

            await Task.CompletedTask;
        }
    }
}
