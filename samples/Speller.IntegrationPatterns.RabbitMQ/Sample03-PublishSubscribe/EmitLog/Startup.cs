using Microsoft.Extensions.DependencyInjection;

namespace EmitLog
{
    internal static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
            => services
                .AddRabbitMQBusService(
                    service => service
                        .ConnectionString("amqp://localhost")
                        .DefaultChannel(
                            channel => channel
                                .DeclareQueue()
                                .DeclareExchange("logs", type: "fanout")
                                .MapRoute<string>("", "logs")
                        )
                );
    }
}
