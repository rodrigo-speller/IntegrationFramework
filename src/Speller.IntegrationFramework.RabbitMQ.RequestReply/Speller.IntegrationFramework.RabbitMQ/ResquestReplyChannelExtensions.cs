// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.RequestReply;
using System;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ
{
    public static class ResquestReplyChannelExtensions
    {
        public static async Task<RabbitMQDelivery> Request<TMessage>(this IRabbitMQChannel channel, TMessage message, string exchange = null, string routingKey = null)
        {
            if (channel == null)
                throw new NullReferenceException();

            var content = channel.Format(message);

            return await channel.Request(content, exchange, routingKey);
        }

        public static async Task<RabbitMQDelivery> Request(this IRabbitMQChannel channel, IMessageContent content, string exchange = null, string routingKey = null)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var model = new RequestReplyModel(content);

            await channel.Send(model, routingKey, exchange);

            return await model.Context.Task;
        }
    }
}
