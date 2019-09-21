// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal sealed class QueueConsumerSubscriber : IConsumer
    {
        private readonly string queue;
        private readonly AcknowledgeMode acknowledgeMode;
        private readonly DeliveryHandler deliveryHandler;

        public QueueConsumerSubscriber(
            string queue,
            AcknowledgeMode acknowledgeMode,
            ExceptionMode exceptionMode,
            IMessageHandler<RabbitMQDelivery> handler,
            AsyncContextAccessor asyncContextAccessor)
        {
            this.queue = queue;
            this.acknowledgeMode = acknowledgeMode;

            deliveryHandler = new DeliveryHandler(acknowledgeMode, exceptionMode, handler, asyncContextAccessor);
        }

        public Task Initialize(RabbitMQChannel channel)
            => Task.Run(() => {
                var model = channel.Model;

                var consumer = new EventingBasicConsumer(model);
                consumer.Received += (_, ea)
                    => deliveryHandler.Handle(channel, ea)
                        .GetAwaiter()
                        .GetResult();

                model.BasicConsume(
                    queue: queue,
                    autoAck: acknowledgeMode == AcknowledgeMode.Automatic,
                    consumer: consumer
                );
            });
    }
}
