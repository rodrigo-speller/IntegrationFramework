// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQChannelOptionsBuilderExchangeExtensions
    {
        public static RabbitMQChannelOptionsBuilder DeclareExchange(
            this RabbitMQChannelOptionsBuilder builder,
            string exchange,
            string type,
            bool durable = false,
            bool autoDelete = false,
            IDictionary<string, object> arguments = null,
            Action<RabbitMQExchangeOptionsBuilder> optionsAction = null)
        {
            if (exchange == null)
                throw new ArgumentNullException(nameof(exchange));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            builder.ExchangesOptionsFactories.Add(() =>
            {
                var exchangeBuilder = new RabbitMQExchangeOptionsBuilder(exchange, type, durable, autoDelete, arguments);
                optionsAction?.Invoke(exchangeBuilder);
                return exchangeBuilder.Build();
            });

            return builder;
        }
    }
}
