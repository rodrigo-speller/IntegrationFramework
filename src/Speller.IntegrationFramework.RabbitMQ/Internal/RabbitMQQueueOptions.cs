// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class RabbitMQQueueOptions
    {
        public RabbitMQQueueOptions(
            string queue,
            bool durable,
            bool exclusive,
            bool autoDelete,
            IDictionary<string, object> arguments,
            Func<IServiceProvider, Task>[] onDeclaredActions)
        {
            Queue = queue;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Arguments = arguments;
            OnDeclaredActions = onDeclaredActions;
        }

        public string Queue { get; }
        public bool Durable { get; }
        public bool Exclusive { get; }
        public bool AutoDelete { get; }
        public IDictionary<string, object> Arguments { get; }
        public IEnumerable<Func<IServiceProvider, Task>> OnDeclaredActions { get; }
    }
}
