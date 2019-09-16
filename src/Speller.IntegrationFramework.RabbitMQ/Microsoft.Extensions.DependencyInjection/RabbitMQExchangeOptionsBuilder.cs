// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.Internal;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public class RabbitMQExchangeOptionsBuilder
    {
        private readonly string exchange;
        private readonly string type;
        private readonly bool durable;
        private readonly bool autoDelete;
        private readonly IDictionary<string, object> arguments;

        internal RabbitMQExchangeOptionsBuilder(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            this.exchange = exchange;
            this.type = type;
            this.durable = durable;
            this.autoDelete = autoDelete;
            this.arguments = arguments;
        }

        internal RabbitMQExchangeOptions Build()
            => new RabbitMQExchangeOptions(exchange, type, durable, autoDelete, arguments);
    }
}
