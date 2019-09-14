// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class LambdaMessageHandler : IMessageHandler<RabbitMQDelivery>
    {
        private readonly Func<RabbitMQDelivery, Task> handler;

        public LambdaMessageHandler(Func<RabbitMQDelivery, Task> handler)
        {
            this.handler = handler;
        }
        
        public Task Handle(RabbitMQDelivery message)
            => handler(message);
    }
}
