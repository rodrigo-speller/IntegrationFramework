// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal sealed class RabbitMQChannel : IRabbitMQChannel
    {
        public RabbitMQChannel(RabbitMQChannelOptions options, IModel model)
        {
            Options = options;
            Model = model;

            Sender = new RabbitMQMessageSender(this);
        }

        public RabbitMQChannelOptions Options { get; }
        public object Tag => Options.Tag;

        public IModel Model { get; }
        public RabbitMQMessageSender Sender { get; }

        public IMessageContent Format<TMessage>(TMessage message)
            => Options.MessageTypeOptionsProvider.Get<TMessage>().GetContent(message);

        public Task Send<TMessage>(TMessage message, string routingKey, string exchange)
            => Sender.Send(message, routingKey, exchange);

        internal Task Subscribe(IConsumer consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException(nameof(consumer));

            return Subscribe((_) => consumer);
        }

        internal async Task Subscribe(Func<IServiceProvider, IConsumer> consumerFactory)
        {
            var services = Options.Services;

            services.GetContextAccessor()
                .Set(this);
                
            var consumer = consumerFactory(services);

            await consumer.Initialize(this);
        }
    }
}
