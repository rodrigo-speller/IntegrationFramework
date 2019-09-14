using Microsoft.Extensions.DependencyInjection;

namespace Receive
{
    internal static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
            => services
                .AddRabbitMQBusService(
                    service => service
                        .ConnectionString("amqp://localhost")
                        .AddChannel(
                            channel => channel
                                .Subscribe<Receive>()
                        )
                );
    }
}
