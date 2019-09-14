using Microsoft.Extensions.DependencyInjection;

namespace RpcClient
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
                                .AddRequestReply()
                        )
                );
    }
}
