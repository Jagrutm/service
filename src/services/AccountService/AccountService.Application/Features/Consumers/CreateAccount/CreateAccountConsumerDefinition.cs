using EventBus.Messages.Common;
using MassTransit;

namespace AccountService.Application.Features.Consumers.CreateAccount
{
    public class CreateAccountConsumerDefinition : ConsumerDefinition<CreateAccountConsumer>
    {
        public CreateAccountConsumerDefinition()
        {
            EndpointName = EventBusConstants.AccountCreateQueue;

            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            ConcurrentMessageLimit = EventBusConstants.DefaultConcurrentMessageLimit;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<CreateAccountConsumer> consumerConfigurator)
        {
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 1000));

            //// use the outbox to prevent duplicate events from being published
            //endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
