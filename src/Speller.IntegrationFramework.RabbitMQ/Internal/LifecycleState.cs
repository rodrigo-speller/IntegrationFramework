// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal enum LifecycleState : int
    {
        Undefined = -1,

        PreInitialization = 0,
        CreateChannels = 1,
        DeclareEntities = 2,
        BindEntities = 3,
        PostInitialization = 4,

        PreConfiguration = 5,
        SubscribeQueues = 6,
        PostConfiguration = 7,

        Shutdown = int.MaxValue,
    }
}
