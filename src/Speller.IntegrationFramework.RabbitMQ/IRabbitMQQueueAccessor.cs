// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Speller.IntegrationFramework.RabbitMQ
{
    public interface IRabbitMQQueueAccessor
    {
        IRabbitMQQueue Queue { get; set; }
    }
}
