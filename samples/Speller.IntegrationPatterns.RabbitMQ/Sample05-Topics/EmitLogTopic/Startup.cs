using Microsoft.Extensions.DependencyInjection;

namespace EmitLogTopic
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
                                .DeclareExchange("topic_logs", type: "topic")
                                .MapRoute<string>("", "topic_logs")
                        )
                );
    }
}
