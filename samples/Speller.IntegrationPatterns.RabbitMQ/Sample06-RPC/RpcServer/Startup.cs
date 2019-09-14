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
                                .Subscribe<RpcServer>()
                        )
                );
    }
}
