// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class RabbitMQChannelOptions
    {
        public RabbitMQChannelOptions(
            object tag,
            IServiceProvider services,
            Func<IServiceProvider, ISubscriber>[] subscribersFactories,
            MessageTypeOptionsProvider messageTypeOptionsProvider,
            RabbitMQExchangeOptions[] exchangesOptions,
            RabbitMQQueueOptions[] queuesOptions)
        {
            Tag = tag;
            Services = services;
            SubscribersFactories = subscribersFactories;
            MessageTypeOptionsProvider = messageTypeOptionsProvider;
            ExchangesOptions = exchangesOptions;
            QueuesOptions = queuesOptions;
        }
        
        public object Tag { get; }
        public IServiceProvider Services { get; }
        public MessageTypeOptionsProvider MessageTypeOptionsProvider { get; }
        public IEnumerable<RabbitMQExchangeOptions> ExchangesOptions { get; }
        public IEnumerable<RabbitMQQueueOptions> QueuesOptions { get; }
        public IEnumerable<Func<IServiceProvider, ISubscriber>> SubscribersFactories { get; }
    }
}
