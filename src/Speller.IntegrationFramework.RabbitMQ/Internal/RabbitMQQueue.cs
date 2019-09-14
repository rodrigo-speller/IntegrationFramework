// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class RabbitMQQueue : IRabbitMQQueue
    {
        private readonly RabbitMQQueueOptions options;

        public RabbitMQQueue(RabbitMQQueueOptions options, string name)
        {
            this.options = options;
            Name = name;
        }

        public string Name { get; }
    }
}
