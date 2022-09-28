using MassTransit;
using POC.Choreography.Events;

namespace POC.Choreography.Payment.Server.Consumers
{
    public class PaymentRequestConsumer : IConsumer<PaymentRequestedEvent>
    {
        private IPublishEndpoint _publishEndpoint;
        public PaymentRequestConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<PaymentRequestedEvent> context)
        {
            Console.WriteLine("Received payment request for order " + context.Message.OrderId);
            await DoPayment();
            Console.WriteLine("Payment request completed for order " + context.Message.OrderId);
            Console.WriteLine("Requesting shipping for order " + context.Message.OrderId );
            await _publishEndpoint.Publish(new OrderShipmentStartedEvent { OrderId = context.Message.OrderId });
            await _publishEndpoint.Publish(new ReportingEvent { OrderId = context.Message.OrderId, OrderStatus = "ShippingStarted"});
        }

        private Task DoPayment()
        {
            return Task.CompletedTask;
        }
    }
}
