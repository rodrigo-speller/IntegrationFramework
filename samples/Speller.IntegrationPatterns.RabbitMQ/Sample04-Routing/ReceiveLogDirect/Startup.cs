using Microsoft.Extensions.DependencyInjection;

namespace ReceiveLogsDirect
{
    internal class Startup
    {
        private readonly string[] severities;

        public Startup(string[] severities)
        {
            this.severities = severities;
        }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddRabbitMQBusService(
                    service => service
                        .ConnectionString("amqp://localhost")
                        .DefaultChannel(
                            channel => channel
                                .DeclareQueue(
                                    queue => {
                                        foreach (var severity in severities)
                                            queue.Bind("direct_logs", severity);

                                        queue.Subscribe<ReceiveLogsDirect>();
                                    }
                                )
                        )
                );
    }
}
