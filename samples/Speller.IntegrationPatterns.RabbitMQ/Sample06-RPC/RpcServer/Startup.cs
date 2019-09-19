using Microsoft.Extensions.DependencyInjection;

namespace RpcServer
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
                                .PrefetchPerConsumer(1)
                                .DeclareQueue(
                                    "rpc_queue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false
                                )
                                .Subscribe<RpcServer>()
                        )
                );
    }
}
