// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.Content;

namespace Speller.IntegrationFramework.RabbitMQ
{
    public interface IMessageContent
    {
        MessageProperties Properties { get; }
        byte[] GetBody();
    }
}
