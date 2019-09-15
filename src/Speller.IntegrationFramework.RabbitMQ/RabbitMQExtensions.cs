// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ
{
    public static class RabbitMQExtensions
    {
        public static Task Publish<TMessage>(this IRabbitMQChannel channel, TMessage message, string routingKey = null, string exchange = null)
            => channel.Publish(message, routingKey, exchange);
    }
}
