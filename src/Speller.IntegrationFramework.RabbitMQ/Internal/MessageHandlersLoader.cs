// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal static class MessageHandlersLoader
    {
        private static readonly MethodInfo MessageHandlerMethod = typeof(IMessageHandler<RabbitMQDelivery>)
            .GetMethod(nameof(IMessageHandler<RabbitMQDelivery>.Handle));

        internal static IEnumerable<Func<IServiceProvider, ISubscriber>> LoadFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes)
            {
                if (!IsMessageHandler(type))
                    continue;

                yield return services => BuildMessageHandler(services, type);
            }
        }

        internal static Func<IServiceProvider, ISubscriber> LoadFromType(Type type)
        {
            if (!IsMessageHandler(type.GetTypeInfo()))
                throw new ArgumentException(null, nameof(type));

            return services => BuildMessageHandler(services, type);
        }

        internal static ISubscriber BuildMessageHandler(IServiceProvider services, Type type)
        {
            var handler = (IMessageHandler<RabbitMQDelivery>)ActivatorUtilities.CreateInstance(services, type);
            if (handler is ISubscriber subscriber)
                return subscriber;

            var acknowledgeMode = AcknowledgeModeAttribute.FromType(type);
            var exceptionMode = ExceptionModeAttribute.FromType(type);
            var subscription = type.GetCustomAttribute<SubscribeAttribute>(true);
            var asyncContextAccessor = services.GetRequiredService<AsyncContextAccessor>();

            if (subscription == null)
            {
                var exchangeBinding = type.GetCustomAttribute<ExchangeBindingAttribute>(true);
                if (exchangeBinding != null)
                    return new ExchangeBindingSubscriber(
                        exchangeBinding.Exchange,
                        exchangeBinding.RoutingKey,
                        acknowledgeMode,
                        exceptionMode,
                        handler,
                        asyncContextAccessor);

                subscription = SubscribeAttribute.FromType(type);
            }

            return new QueueConsumerSubscriber(subscription.Queue, acknowledgeMode, exceptionMode, handler, asyncContextAccessor);
        }

        internal static ISubscriber BuildMessageHandlerToQueueDeclare(string queue, IServiceProvider services, Type type)
        {
            var handler = (IMessageHandler<RabbitMQDelivery>)ActivatorUtilities.CreateInstance(services, type);
            if (handler is ISubscriber subscriber)
                return subscriber;

            var asyncContextAccessor = services.GetRequiredService<AsyncContextAccessor>();

            var acknowledgeMode = AcknowledgeModeAttribute.FromType(type);
            var exceptionMode = ExceptionModeAttribute.FromType(type);

            return new QueueConsumerSubscriber(queue, acknowledgeMode, exceptionMode, handler, asyncContextAccessor);
        }

        internal static bool IsMessageHandler(Type type)
            => IsMessageHandler(type.GetTypeInfo());

        internal static bool IsMessageHandler(TypeInfo type)
            => type.IsClass
            && !type.IsAbstract
            && type.ImplementedInterfaces.Contains(typeof(IMessageHandler<RabbitMQDelivery>));
    }
}
