// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class RabbitMQBusServiceOptions
    {
        internal RabbitMQBusServiceOptions(
            IServiceProvider serviceProvider,
            ConnectionFactory connectionFactory,
            object defaultChannelTag,
            MessageTypeOptionsProvider messageTypeOptionsProvider,
            RabbitMQChannelOptions[] channels)
        {
            DefaultChannelsTag = defaultChannelTag;
            Services = serviceProvider;
            ConnectionFactory = connectionFactory;
            MessageTypeOptionsProvider = messageTypeOptionsProvider;
            Channels = channels;
        }

        public IServiceProvider Services { get; }
        public ConnectionFactory ConnectionFactory { get; }
        public MessageTypeOptionsProvider MessageTypeOptionsProvider { get; }
        public IEnumerable<RabbitMQChannelOptions> Channels { get; }
        public object DefaultChannelsTag { get; }
    }
}
