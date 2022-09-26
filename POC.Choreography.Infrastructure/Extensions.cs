using MassTransit;

namespace POC.Choreography.Infrastructure
{
    public static class Extensions
    {
        public static void ConfigureRabbitmq(this IBusRegistrationConfigurator busRegistrationConfigurator)
        {
            busRegistrationConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        }
    }
}