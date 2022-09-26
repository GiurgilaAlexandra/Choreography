using MassTransit;
using POC.Choreography.Events;

namespace POC.Choreography.Shipping.Server.Consumers
{
    public class OrderShipmentStartedEventConsumer : IConsumer<OrderShipmentStartedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderShipmentStartedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderShipmentStartedEvent> context)
        {
            Console.WriteLine("Received shipping request for order " + context.Message.OrderId);
            await DoShipping();

            await _publishEndpoint.Publish(new OrderShipmentCompletedEvent { OrderId = context.Message.OrderId });
            Console.WriteLine("Shipping completed for order " + context.Message.OrderId);
        }

        private Task DoShipping()
        {
            for (var i = 1; i <= 10; i++)
            {
                Console.WriteLine("Shipping...");
                Thread.Sleep(1_000);
            }

            return Task.CompletedTask;
        }
    }
}
