// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class LifecycleController
    {
        private const LifecycleState InitialState = LifecycleState.PreInitialization;
        private const LifecycleState FinalState = LifecycleState.PostConfiguration;

        public LifecycleState CurrentState { get; private set; } = LifecycleState.Undefined;

        private readonly IDictionary<LifecycleState, ConcurrentQueue<Func<Task>>> pipeline
            = new [] 
            {
                LifecycleState.PreInitialization,
                LifecycleState.CreateChannels,
                LifecycleState.DeclareEntities,
                LifecycleState.BindEntities,
                LifecycleState.PostInitialization,
                
                LifecycleState.PreConfiguration,
                LifecycleState.SubscribeQueues,
                LifecycleState.PostConfiguration,
                
                LifecycleState.Shutdown
            }.ToDictionary(x => x, x => new ConcurrentQueue<Func<Task>>());
        
        public void On(LifecycleState state, Func<Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (CurrentState >= state)
                throw new InvalidOperationException();

            pipeline[state].Enqueue(action);
        }
        
        public async Task Start()
        {
            for (var state = InitialState; state < FinalState; state++)
            {
                CurrentState = state;
                var actions = pipeline[state];
                foreach(var action in actions)
                    await action();
            }
        }

        public async Task Stop()
        {
            var actions = pipeline[LifecycleState.Shutdown];
            foreach (var action in actions)
                await action();
        }
    }
}
