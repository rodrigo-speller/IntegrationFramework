// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ;
using Speller.IntegrationFramework.RabbitMQ.Internal;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQChannelOptionsBuilderMessageHandlersExtensions
    {
        public static RabbitMQChannelOptionsBuilder Subscribe(this RabbitMQChannelOptionsBuilder builder, Assembly assembly)
        {
            var subscriberFactories = MessageHandlersLoader.LoadFromAssembly(assembly);

            foreach (var subscriberFactory in subscriberFactories)
                builder.SubscribersFactories.Add(subscriberFactory);

            return builder;
        }

        public static RabbitMQChannelOptionsBuilder Subscribe<THandler>(this RabbitMQChannelOptionsBuilder builder)
            where THandler : IMessageHandler<RabbitMQDelivery>
            => builder.Subscribe(typeof(THandler));

        public static RabbitMQChannelOptionsBuilder Subscribe(this RabbitMQChannelOptionsBuilder builder, Type handlerType)
        {
            if (!MessageHandlersLoader.IsMessageHandler(handlerType))
                throw new ArgumentException("The supplied type is not a valid message handler.", nameof(handlerType));

            builder.SubscribersFactories.Add(services => MessageHandlersLoader.BuildMessageHandler(services, handlerType));

            return builder;
        }
    }
}
