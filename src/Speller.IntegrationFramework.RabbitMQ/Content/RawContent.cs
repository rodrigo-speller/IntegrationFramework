// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Speller.IntegrationFramework.RabbitMQ.Content
{
    public class RawContent : IMessageContent
    {
        private readonly byte[] body;

        public RawContent(byte[] body)
            : this(body, new MessageProperties())
        { }

        public RawContent(byte[] body, MessageProperties properties)
        {
            this.body = body
                ?? Array.Empty<byte>();

            Properties = properties
                ?? new MessageProperties();
        }

        public MessageProperties Properties { get; }

        public virtual byte[] GetBody()
            => body;
    }
}
