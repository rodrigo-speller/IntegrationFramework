﻿// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Speller.IntegrationFramework
{
    public interface IPublishSubscribeChannel
    {
        Task Publish<TMessage>(TMessage message);
        Task Subscribe<TMessage>(IMessageHandler<TMessage> handler);
    }
}
