using MassTransit;
using Microsoft.AspNetCore.Mvc;
using POC.Choreography.Events;
using POC.Choreography.Order.Service.Models;
using POC.Orchestration.Order.Service.Models;

namespace POC.Choreography.Order.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRequestClient<PaymentRequestedEvent> _paymentClient;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderController(IRequestClient<PaymentRequestedEvent> paymentClient,
            IPublishEndpoint publishEndpoint)
        {
            _paymentClient = paymentClient;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int orderId)
        {
            return Ok(new OrderModel());
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateOrderModel createOrderModel)
        {
            var order = CreateNewOrder(createOrderModel);
         await SaveOrder(order);
         Console.WriteLine("Created order number: " + order.Id);
         await NotifyClient(order.Id, "Created");

         await PublishPaymentRequestedEvent(order);
         await NotifyClient(order.Id, "PaymentRequested");
         //await NotifyClient(order.Id, "PaymentCompleted");

         //await PublishOrderShipmentStartedEvent(order);
         //await NotifyClient(order.Id, "ShippingStarted");

         //Console.WriteLine("Shipping completed for order " + order.Id);
         //await NotifyClient(order.Id, "ShippingCompleted");
         //await CloseOrder(order.Id);
         //await NotifyClient(order.Id, "Closed");

         return Accepted();
        }

        private async Task PublishPaymentRequestedEvent(OrderModel order)
        {
            Console.WriteLine("Requesting payment for order " + order.Id);
            await _publishEndpoint.Publish(new PaymentRequestedEvent { OrderId = order.Id });
        }

        private async Task PublishOrderShipmentStartedEvent(OrderModel order)
        {
            Console.WriteLine("Requesting shipping for order " + order.Id);
            await _publishEndpoint.Publish(new OrderShipmentStartedEvent { OrderId = order.Id });
        }

        private async Task NotifyClient(int orderId, string status)
        {
            await _publishEndpoint.Publish(new ReportingEvent { OrderId = orderId, OrderStatus = status });
        }

        private Task<OrderModel> SaveOrder(OrderModel order)
        {
            order.Id = Random.Shared.Next(10);
            return Task.FromResult(order);
        }

        private OrderModel CreateNewOrder(CreateOrderModel createOrderModel)
        {
            return new OrderModel();
        }

        private Task CloseOrder(int orderId)
        {
            Console.WriteLine("Closing order " + orderId);
            return Task.CompletedTask;
        }
    }
}