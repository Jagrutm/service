using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using System.Diagnostics;
using System.Text.Json;

namespace AccountService.Application.Features.Consumers.UpdateAccount
{
    public class UpdateAccountConsumer : IConsumer<MessageEvent>
    {
        public async Task Consume(ConsumeContext<MessageEvent> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Console.WriteLine(EventBusConstants.GetMessage(context, EventBusConstants.AccountUpdateQueue));
            Debug.WriteLine(EventBusConstants.GetMessage(context, EventBusConstants.AccountUpdateQueue));

            //do-something with context.Message.Message

            await Task.CompletedTask;
        }
    }
}
