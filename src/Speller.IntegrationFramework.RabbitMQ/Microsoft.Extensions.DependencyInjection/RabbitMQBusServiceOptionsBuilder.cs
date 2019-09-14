// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.Internal;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public class RabbitMQBusServiceOptionsBuilder
    {
        internal RabbitMQBusServiceOptionsBuilder(IServiceProvider services)
        {
            Services = services;
            Strict = false;
        }

        internal object DefaultChannelTag { get; set; }

        internal bool Strict { get; set; }
        
        internal ICollection<Func<MessageTypeOptionsProvider, RabbitMQChannelOptions>> ChannelsOptionsFactories { get; }
            = new LinkedList<Func<MessageTypeOptionsProvider, RabbitMQChannelOptions>>();

        internal ICollection<Action<ConnectionFactory>> ConfigureConnectionActions { get; }
            = new LinkedList<Action<ConnectionFactory>>();

        internal IServiceProvider Services { get; }

        internal RabbitMQBusServiceOptions Build()
        {
            var strict = Strict;
            object defaultChannelTag = DefaultChannelTag;
            var connectionFactory = new ConnectionFactory();

            var messageTypeOptionsProvider = strict
                ? null
                : MessageTypeOptionsProvider.CreateDefaultProvider();

            foreach (var action in ConfigureConnectionActions)
                action(connectionFactory);

            var channelsOptions = ChannelsOptionsFactories
                .Select(factory => factory(messageTypeOptionsProvider))
                .ToArray();

            if (!strict)
            {
                bool autoDefaultChannel;
                if (defaultChannelTag == null)
                {
                    defaultChannelTag = new object();
                    autoDefaultChannel = true;
                }
                else
                {
                    autoDefaultChannel = !channelsOptions.Any(x => defaultChannelTag.Equals(x.Tag));
                }

                if (autoDefaultChannel)
                {
                    Array.Resize(ref channelsOptions, channelsOptions.Length + 1);
                    channelsOptions[channelsOptions.Length - 1] = new RabbitMQChannelOptionsBuilder(Services, defaultChannelTag, messageTypeOptionsProvider).Build();
                }
            }

            return new RabbitMQBusServiceOptions(
                Services,
                connectionFactory,
                defaultChannelTag,
                messageTypeOptionsProvider,
                channelsOptions
            );
        }
    }
}
