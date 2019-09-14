using Microsoft.Extensions.DependencyInjection;

namespace ReceiveLogsTopic
{
    internal class Startup
    {
        private readonly string[] bindingKeys;

        public Startup(string[] bindingKeys)
        {
            this.bindingKeys = bindingKeys;
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
                                        foreach (var bindingKey in bindingKeys)
                                            queue.Bind("topic_logs", bindingKey);

                                        queue.Subscribe<ReceiveLogsTopic>();
                                    }
                                )
                        )
                );
    }
}
