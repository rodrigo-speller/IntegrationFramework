// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ;
using Speller.IntegrationFramework.RabbitMQ.Internal;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQExtensions
    {
        public static IServiceCollection AddRabbitMQBusService(this IServiceCollection services, Action<RabbitMQBusServiceOptionsBuilder> optionsAction)
        {
            services
                .AddSingleton(provider => {
                    var builder = new RabbitMQBusServiceOptionsBuilder(provider);

                    optionsAction?.Invoke(builder);

                    return builder.Build();
                })
                .AddSingleton(provider => new RabbitMQBusService(provider.GetService<RabbitMQBusServiceOptions>()))
                .AddTransient<IHostedService>(provider => provider.GetService<RabbitMQBusService>())
                .AddSingleton<IRabbitMQBusService>(provider => provider.GetService<RabbitMQBusService>())
                .AddSingleton<LifecycleController>()
                .AddSingleton<AsyncContextAccessor>()
                .AddSingleton<IRabbitMQChannelAccessor>(provider => provider.GetContextAccessor())
                .AddSingleton<IRabbitMQQueueAccessor>(provider => provider.GetContextAccessor())
                .AddTransient<IRabbitMQChannel>(provider => provider.GetService<IRabbitMQChannelAccessor>().Channel
                    ?? provider.GetService<IRabbitMQBusService>().DefaultChannel)
                .AddTransient<IRabbitMQQueue>(provider => provider.GetService<IRabbitMQQueueAccessor>().Queue)
                .AddTransient<IPublishSubscribeChannel>(provider => provider.GetService<IRabbitMQChannel>())
                ;

            return services;
        }

        internal static AsyncContextAccessor GetContextAccessor(this IServiceProvider services)
            => services.GetService<AsyncContextAccessor>();

        internal static void Set(this AsyncContextAccessor accessor, IRabbitMQChannel channel)
            => accessor.Set(channel, null);

        internal static void Set(this AsyncContextAccessor accessor, IRabbitMQQueue queue)
            => accessor.Set(null, queue);
    }
}
