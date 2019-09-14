// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQChannelOptionsBuilderMessageTypeExtensions
    {
        public static RabbitMQChannelOptionsBuilder Map<TMessage>(this RabbitMQChannelOptionsBuilder builder, Func<TMessage, IMessageContent> formatter)
        {
            var typeOptions = builder.MessageTypeOptionsProvider.Get<TMessage>();

            typeOptions.Formatter = formatter == null
                ? (Func<object, IMessageContent>)null
                : (src) => formatter((TMessage)src);

            return builder;
        }

        public static RabbitMQChannelOptionsBuilder MapRoute<TMessage>(this RabbitMQChannelOptionsBuilder builder, string defaultRoutingKey)
        {
            var typeOptions = builder.MessageTypeOptionsProvider.Get<TMessage>();

            typeOptions.DefaultRoutingKey = defaultRoutingKey;

            return builder;
        }


        public static RabbitMQChannelOptionsBuilder MapRoute<TMessage>(this RabbitMQChannelOptionsBuilder builder, string defaultRoutingKey, string defaultExchange)
        {
            var typeOptions = builder.MessageTypeOptionsProvider.Get<TMessage>();

            typeOptions.DefaultExchange = defaultExchange;
            typeOptions.DefaultRoutingKey = defaultRoutingKey;
            
            return builder;
        }

        public static RabbitMQChannelOptionsBuilder MapRoute<TMessage>(this RabbitMQChannelOptionsBuilder builder, string defaultRoutingKey, Func<TMessage, IMessageContent> formatter)
        {
            var typeOptions = builder.MessageTypeOptionsProvider.Get<TMessage>();

            typeOptions.DefaultRoutingKey = defaultRoutingKey;
            typeOptions.Formatter = formatter == null
                ? (Func<object, IMessageContent>)null
                : (src) => formatter((TMessage)src);

            return builder;
        }

        public static RabbitMQChannelOptionsBuilder MapRoute<TMessage>(this RabbitMQChannelOptionsBuilder builder, string defaultRoutingKey, string defaultExchange, Func<TMessage, IMessageContent> formatter)
        {
            var typeOptions = builder.MessageTypeOptionsProvider.Get<TMessage>();

            typeOptions.DefaultExchange = defaultExchange;
            typeOptions.DefaultRoutingKey = defaultRoutingKey;
            typeOptions.Formatter = formatter == null
                ? (Func<object, IMessageContent>)null
                : (src) => formatter((TMessage)src);

            return builder;
        }
    }
}
