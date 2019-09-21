// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal sealed class ExchangeBindingSubscriber : IConsumer
    {
        private readonly string exchange;
        private readonly string routingKey;
        private readonly AcknowledgeMode acknowledgeMode;
        private readonly ExceptionMode exceptionMode;
        private readonly DeliveryHandler deliveryHandler;

        public ExchangeBindingSubscriber(
            string exchange,
            string routingKey,
            AcknowledgeMode acknowledgeMode,
            ExceptionMode exceptionMode,
            IMessageHandler<RabbitMQDelivery> handler,
            AsyncContextAccessor asyncContextAccessor)
        {
            this.exchange = exchange ?? string.Empty;
            this.routingKey = routingKey ?? string.Empty;
            this.acknowledgeMode = acknowledgeMode;
            this.exceptionMode = exceptionMode;

            deliveryHandler = new DeliveryHandler(acknowledgeMode, exceptionMode, handler, asyncContextAccessor);
        }

        public Task Initialize(RabbitMQChannel channel)
            => Task.Run(() => {
                var model = channel.Model;

                var queueName = model.QueueDeclare().QueueName;

                model.QueueBind(
                    queue: queueName,
                    exchange: exchange,
                    routingKey: routingKey
                );

                var consumer = new EventingBasicConsumer(model);
                consumer.Received += (_, ea)
                    => deliveryHandler.Handle(channel, ea)
                        .GetAwaiter()
                        .GetResult();

                model.BasicConsume(
                    queue: queueName,
                    autoAck: acknowledgeMode == AcknowledgeMode.Automatic,
                    consumer: consumer
                );
            });
    }
}
