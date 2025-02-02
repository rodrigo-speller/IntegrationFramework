﻿using Microsoft.Extensions.DependencyInjection;

namespace Send
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
                                .MapRoute<string>("hello")
                        )
                );
    }
}
