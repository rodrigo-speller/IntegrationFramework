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
                        .DefaultChannel(
                            channel => channel
                                .DeclareQueue("hello", exclusive: false)
                                .Subscribe<Receive>()
                        )
                );
    }
}
