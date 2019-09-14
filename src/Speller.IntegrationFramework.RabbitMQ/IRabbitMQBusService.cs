// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Speller.IntegrationFramework.RabbitMQ
{
    public interface IRabbitMQBusService
    {
        IRabbitMQChannel DefaultChannel { get; }
        IEnumerable<IRabbitMQChannel> Channels { get; }

        IRabbitMQChannel GetChannel(object tag);
    }
}
