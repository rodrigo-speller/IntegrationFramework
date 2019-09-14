// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Speller.IntegrationFramework.RabbitMQ.RequestReply
{
    internal class RequestReplyModel
    {
        public RequestReplyModel(IMessageContent sourceContent)
        {
            SourceContent = sourceContent;
        }

        public IMessageContent SourceContent { get; }

        public RequestReplyContext Context { get; set; }
    }
}
