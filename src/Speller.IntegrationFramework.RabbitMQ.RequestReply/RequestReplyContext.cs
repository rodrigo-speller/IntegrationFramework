// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.RequestReply
{
    internal class RequestReplyContext
    {
        private readonly TaskCompletionSource<RabbitMQDelivery> taskSource;

        public RequestReplyContext(string replyTo, string correlationId, IMessageContent sourceContent)
        {
            var properties = sourceContent.Properties.Clone();

            properties.ReplyTo = replyTo;
            properties.CorrelationId = correlationId;
            SourceContent = sourceContent;
            RequestContent = new RequestMessageContentWrapper(sourceContent, properties);

            taskSource = new TaskCompletionSource<RabbitMQDelivery>();
        }

        public IMessageContent SourceContent { get; set; }
        public IMessageContent RequestContent { get; }

        public Task<RabbitMQDelivery> Task
            => taskSource.Task;

        public void SetException(IEnumerable<Exception> exceptions)
            => taskSource.SetException(exceptions);

        public void SetResult(RabbitMQDelivery result)
            => taskSource.SetResult(result);

        public void Cancel()
            => taskSource.SetCanceled();
    }
}
