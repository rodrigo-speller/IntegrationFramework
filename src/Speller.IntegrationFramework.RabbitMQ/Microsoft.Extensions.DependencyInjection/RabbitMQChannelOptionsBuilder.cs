// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public class RabbitMQChannelOptionsBuilder
    {
        internal RabbitMQChannelOptionsBuilder(IServiceProvider services, object tag, MessageTypeOptionsProvider baseMessageTypeOptionsProvider)
        {
            Services = services;
            Tag = tag;
            MessageTypeOptionsProvider = new MessageTypeOptionsProvider(baseMessageTypeOptionsProvider);
        }

        public object Tag { get; }
        internal ushort? ChannelPrefetchCount { get; set; }
        internal ushort? ConsumerPrefetchCount { get; set; }

        internal IServiceProvider Services { get; }

        internal MessageTypeOptionsProvider MessageTypeOptionsProvider { get; }

        internal ICollection<Func<IServiceProvider, IConsumer>> SubscribersFactories { get; }
            = new LinkedList<Func<IServiceProvider, IConsumer>>();

        internal ICollection<Func<RabbitMQExchangeOptions>> ExchangesOptionsFactories { get; }
            = new LinkedList<Func<RabbitMQExchangeOptions>>();

        internal ICollection<Func<RabbitMQQueueOptions>> QueuesOptionsFactories { get; }
            = new LinkedList<Func<RabbitMQQueueOptions>>();

        internal RabbitMQChannelOptions Build()
        {
            var exchangesOptions = ExchangesOptionsFactories
                .Select(factory => factory())
                .ToArray();

            var queuesOptions = QueuesOptionsFactories
                .Select(factory => factory())
                .ToArray();

            return new RabbitMQChannelOptions(
                Tag,
                ChannelPrefetchCount,
                ConsumerPrefetchCount,
                Services,
                SubscribersFactories.ToArray(),
                MessageTypeOptionsProvider,
                exchangesOptions,
                queuesOptions
            );
        }
    }
}
