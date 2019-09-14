// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.RequestReply
{
    internal class RequestReplyController
    {
        private readonly string queueName;
        private readonly ConcurrentDictionary<string, RequestReplyContext> contexts
            = new ConcurrentDictionary<string, RequestReplyContext>();

        public RequestReplyController(string queueName)
        {
            this.queueName = queueName;
        }
        
        public RequestReplyContext Request(IMessageContent sourceContent)
        {
            RequestReplyContext context;

            while (true)
            {
                var correlationId = GenerateCorrelationId();
                context = new RequestReplyContext(queueName, correlationId, sourceContent);

                if (contexts.TryAdd(correlationId, context))
                    break;
            }

            return context;
        }

        public async Task OnDelivery(RabbitMQDelivery delivery)
        {
            var correlationId = delivery.CorrelationId;

            if (correlationId == null || !contexts.TryRemove(correlationId, out var context))
            {
                await delivery.TryReject();
                return;
            }
            
            context.SetResult(delivery);
        }

        private string GenerateCorrelationId()
            => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
