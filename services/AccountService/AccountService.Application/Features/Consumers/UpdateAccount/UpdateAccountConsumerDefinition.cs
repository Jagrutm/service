using EventBus.Messages.Common;
using MassTransit;

namespace AccountService.Application.Features.Consumers.UpdateAccount
{
    public class UpdateAccountConsumerDefinition : ConsumerDefinition<UpdateAccountConsumer>
    {
        public UpdateAccountConsumerDefinition()
        {
            EndpointName = EventBusConstants.AccountUpdateQueue;

            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            ConcurrentMessageLimit = EventBusConstants.DefaultConcurrentMessageLimit;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<UpdateAccountConsumer> consumerConfigurator)
        {
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            //// use the outbox to prevent duplicate events from being published
            //endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
