// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal sealed class RabbitMQMessageSender : IMessageSender
    {
        private readonly RabbitMQChannel channel;

        public IMessageChannel Channel => channel;

        public RabbitMQMessageSender(RabbitMQChannel channel)
        {
            this.channel = channel;
        }

        public Task Send<TMessage>(TMessage message)
            => Send(message, null, null);

        public async Task Send<TMessage>(TMessage message, string routingKey, string exchange)
        {
            var typeOptions = channel.Options.MessageTypeOptionsProvider.Get<TMessage>();

            routingKey = routingKey ?? typeOptions.DefaultRoutingKey ?? string.Empty;
            exchange = exchange ?? typeOptions.DefaultExchange ?? string.Empty;

            var content = typeOptions.GetContent(message);

            await Send(content, routingKey, exchange);
        }

        private Task Send(IMessageContent content, string routingKey, string exchange)
            => Task.Run(() => {
                var model = channel.Model;

                var body = content.GetBody();
                var properties = content.Properties.Build(model);
                
                model.BasicPublish(
                    exchange,
                    routingKey,
                    properties,
                    body
                );
            });
    }
}
