// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class AsyncContextAccessor : IRabbitMQChannelAccessor, IRabbitMQQueueAccessor
    {
        private readonly AsyncLocalAccessor<IRabbitMQChannel> channelAccessor
            = new AsyncLocalAccessor<IRabbitMQChannel>();

        private readonly AsyncLocalAccessor<IRabbitMQQueue> queueAccessor
            = new AsyncLocalAccessor<IRabbitMQQueue>();

        public IRabbitMQChannel Channel
        {
            get => channelAccessor.Value;
            set => channelAccessor.Value = value;
        }

        public IRabbitMQQueue Queue
        {
            get => queueAccessor.Value;
            set => queueAccessor.Value = value;
        }

        public void Set(IRabbitMQChannel channel, IRabbitMQQueue queue)
        {
            channelAccessor.Value = channel;
            queueAccessor.Value = queue;
        }
    }
}
