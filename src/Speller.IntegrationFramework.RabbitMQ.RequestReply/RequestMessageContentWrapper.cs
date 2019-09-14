// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.Content;

namespace Speller.IntegrationFramework.RabbitMQ.RequestReply
{
    class RequestMessageContentWrapper : IMessageContent
    {
        public RequestMessageContentWrapper(IMessageContent sourceContent, MessageProperties properties)
        {
            SourceContent = sourceContent;
            Properties = properties;
        }

        public IMessageContent SourceContent { get; set; }

        public MessageProperties Properties { get; }

        public byte[] GetBody()
            => SourceContent.GetBody();
    }
}
