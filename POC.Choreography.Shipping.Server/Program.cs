using MassTransit;
using Microsoft.Extensions.Hosting;
using POC.Choreography.Infrastructure;
using POC.Choreography.Shipping.Server.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.ConfigureRabbitmq();
            x.AddConsumer<OrderShipmentStartedEventConsumer>();
        });

    })
    .Build();

host.Run();