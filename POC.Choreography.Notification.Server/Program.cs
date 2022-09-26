using MassTransit;
using Microsoft.Extensions.Hosting;
using POC.Choreography.Infrastructure;
using POC.Choreography.Notification.Server.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.ConfigureRabbitmq();
            x.AddConsumer<ReportingEventConsumer>();
        });

    })
    .Build();

host.Run();