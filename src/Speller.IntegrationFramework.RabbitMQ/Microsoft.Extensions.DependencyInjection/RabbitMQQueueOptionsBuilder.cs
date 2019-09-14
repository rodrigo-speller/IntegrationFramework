// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class RabbitMQQueueOptionsBuilder
    {
        private readonly string queue;
        private readonly bool durable;
        private readonly bool exclusive;
        private readonly bool autoDelete;
        private readonly IDictionary<string, object> arguments;

        internal RabbitMQQueueOptionsBuilder(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            this.queue = queue;
            this.durable = durable;
            this.exclusive = exclusive;
            this.autoDelete = autoDelete;
            this.arguments = arguments;
        }

        internal ICollection<Func<IServiceProvider, Task>> OnDeclaredActions { get; }
            = new LinkedList<Func<IServiceProvider, Task>>();

        internal ICollection<Func<IServiceProvider, ISubscriber>> MessageHandlersFactories { get; }
            = new LinkedList<Func<IServiceProvider, ISubscriber>>();

        internal RabbitMQQueueOptions Build()
            => new RabbitMQQueueOptions(
                queue, durable, exclusive, autoDelete, arguments,
                OnDeclaredActions.ToArray()
            );
    }
}
