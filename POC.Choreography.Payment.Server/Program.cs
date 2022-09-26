using MassTransit;
using Microsoft.Extensions.Hosting;
using POC.Choreography.Infrastructure;
using POC.Choreography.Payment.Server.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.ConfigureRabbitmq();
            x.AddConsumer<PaymentRequestConsumer>();
        });

    })
    .Build();

host.Run();