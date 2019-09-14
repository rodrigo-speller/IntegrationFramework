// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQBusServiceOptionsBuilderExtensions
    {
        public static RabbitMQBusServiceOptionsBuilder Strict(this RabbitMQBusServiceOptionsBuilder builder)
        {
            builder.Strict = true;

            return builder;
        }

        public static RabbitMQBusServiceOptionsBuilder AddChannel(this RabbitMQBusServiceOptionsBuilder builder, Action<RabbitMQChannelOptionsBuilder> optionsAction)
        {
            builder.AddChannel(null, optionsAction);

            return builder;
        }

        public static RabbitMQBusServiceOptionsBuilder AddChannel(this RabbitMQBusServiceOptionsBuilder builder, object tag, Action<RabbitMQChannelOptionsBuilder> optionsAction)
        {
            if (optionsAction == null)
                throw new ArgumentNullException(nameof(optionsAction));

            if (tag == null)
                tag = new object();

            builder.ChannelsOptionsFactories.Add((messageTypeOptionsProviderBase) => {
                var channelOptionsBuilder = new RabbitMQChannelOptionsBuilder(builder.Services, tag, messageTypeOptionsProviderBase);
                optionsAction(channelOptionsBuilder);
                return channelOptionsBuilder.Build();
            });

            return builder;
        }

        public static RabbitMQBusServiceOptionsBuilder DefaultChannel(this RabbitMQBusServiceOptionsBuilder builder, object tag)
        {
            builder.DefaultChannelTag = tag;

            return builder;
        }

        public static RabbitMQBusServiceOptionsBuilder DefaultChannel(this RabbitMQBusServiceOptionsBuilder builder, Action<RabbitMQChannelOptionsBuilder> optionsAction)
            => builder.DefaultChannel(null, optionsAction);

        public static RabbitMQBusServiceOptionsBuilder DefaultChannel(this RabbitMQBusServiceOptionsBuilder builder, object tag, Action<RabbitMQChannelOptionsBuilder> optionsAction)
        {
            tag = tag ?? new object();

            return builder
                .AddChannel(tag, optionsAction)
                .DefaultChannel(tag);
        }
    }
}
