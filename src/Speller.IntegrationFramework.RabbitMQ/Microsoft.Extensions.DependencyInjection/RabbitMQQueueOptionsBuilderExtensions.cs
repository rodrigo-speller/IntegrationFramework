// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework;
using Speller.IntegrationFramework.RabbitMQ;
using Speller.IntegrationFramework.RabbitMQ.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQQueueOptionsBuilderExtensions
    {
        public static RabbitMQQueueOptionsBuilder Bind(this RabbitMQQueueOptionsBuilder builder, string exchange, string routingKey, IDictionary<string, object> arguments = null)
        {
            builder.OnDeclaredActions.Add((services) => Task.Run(() => {
                var (_, channel, queue) = services.GetContext();

                var startup = services.GetService<LifecycleController>();

                startup.On(LifecycleState.BindEntities, () => Task.Run(() => {
                    channel.Model.QueueBind(queue.Name, exchange, routingKey, arguments);
                }));
            }));
            
            return builder;
        }

        public static RabbitMQQueueOptionsBuilder OnDeclare(this RabbitMQQueueOptionsBuilder builder, Func<IRabbitMQQueue, Task> action)
        {
            builder.OnDeclaredActions.Add(async (services) => {
                var queue = services.GetService<IRabbitMQQueue>();

                await action(queue);
            });

            return builder;
        }

        public static RabbitMQQueueOptionsBuilder Subscribe<THandler>(this RabbitMQQueueOptionsBuilder builder)
            where THandler : IMessageHandler<RabbitMQDelivery>
            => builder.Subscribe(typeof(THandler));

        public static RabbitMQQueueOptionsBuilder Subscribe(this RabbitMQQueueOptionsBuilder builder, Func<RabbitMQDelivery, Task> handler)
            => builder.Subscribe(AcknowledgeModeAttribute.DefaultMode, ExceptionModeAttribute.DefaultMode, handler);

        public static RabbitMQQueueOptionsBuilder Subscribe(this RabbitMQQueueOptionsBuilder builder, AcknowledgeMode acknowledgeMode, Func<RabbitMQDelivery, Task> handler)
            => builder.Subscribe(acknowledgeMode, ExceptionModeAttribute.DefaultMode, handler);

        public static RabbitMQQueueOptionsBuilder Subscribe(this RabbitMQQueueOptionsBuilder builder, AcknowledgeMode acknowledgeMode, ExceptionMode exceptionMode, Func<RabbitMQDelivery, Task> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            builder.OnDeclaredActions.Add((services) => Task.Run(() =>
            {
                var (contextAccessor, channel, queue) = services.GetContext();

                var startup = services.GetService<LifecycleController>();

                startup.On(
                    LifecycleState.SubscribeQueues,
                    () => channel.Subscribe(new QueueConsumerSubscriber(queue.Name, acknowledgeMode, exceptionMode, new LambdaMessageHandler(handler), contextAccessor))
                );
            }));

            return builder;
        }

        public static RabbitMQQueueOptionsBuilder Subscribe(this RabbitMQQueueOptionsBuilder builder, Type handlerType)
        {
            if (!MessageHandlersLoader.IsMessageHandler(handlerType))
                throw new ArgumentException("The supplied type is not a valid message handler.", nameof(handlerType));

            builder.OnDeclaredActions.Add((services) => Task.Run(() => {
                var (_, channel, queue) = services.GetContext();

                var startup = services.GetService<LifecycleController>();

                startup.On(
                    LifecycleState.SubscribeQueues,
                    () => channel.Subscribe(MessageHandlersLoader.BuildMessageHandlerToQueueDeclare(queue.Name, services, handlerType))
                );
            }));

            return builder;
        }

        private static (AsyncContextAccessor, RabbitMQChannel, IRabbitMQQueue) GetContext(this IServiceProvider services)
        {
            var accessor = services.GetRequiredService<AsyncContextAccessor>();
            return (accessor, (RabbitMQChannel)accessor.Channel, accessor.Queue);
        }
    }
}
