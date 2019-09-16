// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class RabbitMQExchangeOptions
    {
        public RabbitMQExchangeOptions(
            string exchange,
            string type,
            bool durable,
            bool autoDelete,
            IDictionary<string, object> arguments)
        {
            Exchange = exchange;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }

        public string Exchange { get; }
        public string Type { get; }
        public bool Durable { get; }
        public bool AutoDelete { get; }
        public IDictionary<string, object> Arguments { get; }
    }
}
