// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal sealed class RabbitMQChannel : IRabbitMQChannel, IMessageContentFormatter
    {
        private readonly List<RabbitMQQueue> queues = new List<RabbitMQQueue>();

        public RabbitMQChannel(RabbitMQChannelOptions options, IModel model)
        {
            Options = options;
            Model = model;
        }

        public object Tag => Options.Tag;
        public IModel Model { get; }

        internal RabbitMQChannelOptions Options { get; }

        public Task Publish<TMessage>(TMessage message)
            => Publish(message, null, null);
        
        public IMessageContent FormatMessageContent<TMessage>(TMessage message)
        {
            var typeOptions = Options.MessageTypeOptionsProvider.Get<TMessage>();

            return FormatMessageContent(message, typeOptions);
        }

        public IMessageContent FormatMessageContent(object message, MessageTypeOptions typeOptions)
        {
            IMessageContent content;
            if (typeOptions.Formatter == null)
            {
                content = message as IMessageContent;

                if (content == null)
                    throw new InvalidOperationException("The message type formatter must be defined.");
            }
            else
            {
                content = typeOptions.Formatter(message);
            }

            return content;
        }

        public async Task Publish<TMessage>(TMessage message, string routingKey, string exchange)
        {
            var typeOptions = Options.MessageTypeOptionsProvider.Get<TMessage>();
            var content = FormatMessageContent(message, typeOptions);

            routingKey = routingKey ?? typeOptions.DefaultRoutingKey ?? string.Empty;
            exchange = exchange ?? typeOptions.DefaultExchange ?? string.Empty;

            await Publish(content, routingKey, exchange);
        }

        private Task Publish(IMessageContent content, string routingKey, string exchange)
        {
            var model = Model;

            var body = content.GetBody();
            var properties = content.Properties.Build(model);

            model.BasicPublish(
                exchange,
                routingKey,
                properties,
                body
            );

            return Task.CompletedTask;
        }

        internal void AddQueue(RabbitMQQueue queue)
            => queues.Add(queue);

        public Task Subscribe<TMessage>(IMessageHandler<TMessage> handler)
        {
            if (handler is ISubscriber subscriber)
                return Subscribe(subscriber);

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            throw new InvalidOperationException();
        }

        internal Task Subscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));

            return Subscribe((_) => subscriber);
        }

        internal async Task Subscribe(Func<IServiceProvider, ISubscriber> subscriberFactory)
        {
            var services = Options.Services;

            services.GetContextAccessor()
                .Set(this);
                
            var subscriber = subscriberFactory(services);

            await subscriber.Initialize(this);
        }
    }
}
