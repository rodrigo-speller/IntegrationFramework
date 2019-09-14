// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Speller.IntegrationFramework.RabbitMQ.Content;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class MessageTypeOptionsProvider
    {
        private readonly MessageTypeOptionsProvider providerBase;
        private readonly IDictionary<Type, MessageTypeOptions> items;

        public MessageTypeOptionsProvider()
            : this(null, new ConcurrentDictionary<Type, MessageTypeOptions>())
        { }

        public MessageTypeOptionsProvider(MessageTypeOptionsProvider providerBase)
            : this(providerBase, new ConcurrentDictionary<Type, MessageTypeOptions>())
        { }

        private MessageTypeOptionsProvider(MessageTypeOptionsProvider providerBase, IDictionary<Type, MessageTypeOptions> items)
        {
            this.items = items;
            this.providerBase = providerBase;
        }

        public MessageTypeOptions Get<T>()
        {
            if (items.TryGetValue(typeof(T), out var options))
                return options;

            return items[typeof(T)] = CreateTypeOptions<T>();
        }

        private MessageTypeOptions CreateTypeOptions<T>()
        {
            if (providerBase != null)
                return providerBase.Get<T>().Clone();
            
            return new MessageTypeOptions();
        }

        public static MessageTypeOptionsProvider CreateDefaultProvider()
        {
            var options = new(Type Type, Func<object, IMessageContent> Formatter)[]
            {
                (typeof(string), x => new StringContent((string)x)),
                (typeof(byte[]), x => new RawContent((byte[])x))
            };

            var items = new ConcurrentDictionary<Type, MessageTypeOptions>(
                options.Select(x =>
                    new KeyValuePair<Type, MessageTypeOptions>(
                        x.Type,
                        new MessageTypeOptions()
                        {
                            Formatter = x.Formatter
                        }
                    )
                )
            );

            return new MessageTypeOptionsProvider(null, items);
        }
    }
}
