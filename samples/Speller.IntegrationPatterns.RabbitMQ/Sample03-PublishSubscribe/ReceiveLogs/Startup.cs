using Microsoft.Extensions.DependencyInjection;

namespace ReceiveLogs
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
                                .DeclareExchange("logs", type: "fanout")
                                .Subscribe<ReceiveLogs>()
                        )
                );
    }
}
