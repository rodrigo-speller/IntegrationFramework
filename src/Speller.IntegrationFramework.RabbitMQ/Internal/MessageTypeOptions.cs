// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class MessageTypeOptions
    {
        public string DefaultRoutingKey { get; internal set; }
        public string DefaultExchange { get; internal set; }
        public Func<object, IMessageContent> Formatter { get; internal set; }

        internal MessageTypeOptions Clone()
            => new MessageTypeOptions()
            {
                DefaultRoutingKey = DefaultRoutingKey,
                DefaultExchange = DefaultExchange,
                Formatter = Formatter
            };
    }
}
