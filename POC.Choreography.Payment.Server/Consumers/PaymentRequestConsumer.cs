using MassTransit;
using POC.Choreography.Events;

namespace POC.Choreography.Payment.Server.Consumers
{
    public class PaymentRequestConsumer : IConsumer<PaymentRequest>
    {
        public async Task Consume(ConsumeContext<PaymentRequest> context)
        {
            Console.WriteLine("Received payment request for order " + context.Message.OrderId);
            await DoPayment();
            await context.RespondAsync(new PaymentResult {IsSucceeded = true});
            
            Console.WriteLine("Payment request completed for order " + context.Message.OrderId);
        }

        private Task DoPayment()
        {
            return Task.CompletedTask;
        }
    }
}
