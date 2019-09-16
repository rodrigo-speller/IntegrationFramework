using Speller.IntegrationFramework.RabbitMQ.Content;
using Microsoft.Extensions.DependencyInjection;

namespace NewTask
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
                                .DeclareQueue(
                                    "task_queue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false
                                )
                                .MapRoute<string>("task_queue", str => {
                                    var content = new StringContent(str);

                                    content.Properties.Persistent = true;

                                    return content;
                                })
                        )
                );
    }
}
