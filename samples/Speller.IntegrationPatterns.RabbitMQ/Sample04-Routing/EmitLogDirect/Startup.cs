using Microsoft.Extensions.DependencyInjection;

namespace EmitLogDirect
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
                                .DeclareExchange("direct_logs", type: "direct")
                                .MapRoute<string>("", "direct_logs")
                        )
                );
    }
}
