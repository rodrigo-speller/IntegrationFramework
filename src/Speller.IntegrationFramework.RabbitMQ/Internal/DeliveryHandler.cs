// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class DeliveryHandler
    {
        private readonly AcknowledgeMode acknowledgeMode;
        private readonly ExceptionMode exceptionMode;
        private readonly IMessageHandler<RabbitMQDelivery> handler;
        private readonly AsyncContextAccessor asyncContextAccessor;

        public DeliveryHandler(
            AcknowledgeMode acknowledgeMode,
            ExceptionMode exceptionMode,
            IMessageHandler<RabbitMQDelivery> handler,
            AsyncContextAccessor asyncContextAccessor)
        {
            this.acknowledgeMode = acknowledgeMode;
            this.exceptionMode = exceptionMode;
            this.handler = handler;
            this.asyncContextAccessor = asyncContextAccessor;
        }
        
        public async Task Handle(RabbitMQChannel channel, BasicDeliverEventArgs source)
        {
            asyncContextAccessor.Set(channel);

            var delivery = new RabbitMQDelivery(channel.Model, acknowledgeMode, source);

            if (acknowledgeMode == AcknowledgeMode.BeforeHandling)
                await delivery.TryAcknowledge();

            try
            {
                await handler.Handle(delivery);
            }
            catch (Exception ex)
            {
                await delivery.SetException(ex);
            }
            
            if (delivery.Exceptions.Any())
            {
                switch (exceptionMode)
                {
                    case ExceptionMode.Hold:
                        return;
                    case ExceptionMode.Reject:
                        await delivery.TryReject();
                        return;
                    case ExceptionMode.Unacknowledge:
                        await delivery.TryUnacknowledge();
                        return;
                    default: // Ignore
                        break;
                }
            }

            if (acknowledgeMode == AcknowledgeMode.AfterHandling)
                await delivery.TryAcknowledge();
        }
    }
}
