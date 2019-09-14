// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQChannelOptionsBuilderQueueExtensions
    {
        public static RabbitMQChannelOptionsBuilder DeclareQueue(this RabbitMQChannelOptionsBuilder builder, Action<RabbitMQQueueOptionsBuilder> optionsAction)
            => builder.DeclareQueue(
                queue: "",
                optionsAction: optionsAction
            );

        public static RabbitMQChannelOptionsBuilder DeclareQueue(
            this RabbitMQChannelOptionsBuilder builder,
            string queue = "",
            bool durable = false,
            bool exclusive = true,
            bool autoDelete = true,
            IDictionary<string, object> arguments = null,
            Action<RabbitMQQueueOptionsBuilder> optionsAction = null)
        {
            builder.QueuesOptionsFactories.Add(() =>
            {
                var queueBuilder = new RabbitMQQueueOptionsBuilder(queue, durable, exclusive, autoDelete, arguments);
                optionsAction?.Invoke(queueBuilder);
                return queueBuilder.Build();
            });

            return builder;
        }
    }
}
