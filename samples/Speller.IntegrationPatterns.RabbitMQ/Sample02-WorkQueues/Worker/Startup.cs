using Microsoft.Extensions.DependencyInjection;

namespace Worker
{
    internal static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
            => services
                .AddRabbitMQBusService(
                    service => service
                        .ConnectionString("amqp://localhost")
                        .DefaultChannel(
                            // TODO: Channel QoS
                            channel => channel
                                .DeclareQueue(
                                    "task_queue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false
                                )
                                .Subscribe<Worker>()
                        )
                );
    }
}
