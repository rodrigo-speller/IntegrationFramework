// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Speller.IntegrationFramework.RabbitMQ.Content
{
    public sealed class MessageProperties
    {
        private IDictionary<string, (object Value, Action<IBasicProperties> Setter)> setters;

        #region Properties

        public string UserId
        {
            get => (string)GetValue(nameof(UserId));
            set => SetValue(nameof(UserId), x => x.UserId = value, value);
        }

        public string ReplyTo
        {
            get => (string)GetValue(nameof(ReplyTo));
            set => SetValue(nameof(ReplyTo), x => x.ReplyTo = value, value);
        }

        public byte? Priority
        {
            get => (byte?)GetValue(nameof(Priority));
            set => SetValue(nameof(Priority), x => x.Priority = value.Value, value);
        }

        public bool? Persistent
        {
            get => (bool?)GetValue(nameof(Persistent));
            set => SetValue(nameof(Persistent), x => x.Persistent = value.Value, value);
        }

        public string MessageId
        {
            get => (string)GetValue(nameof(MessageId));
            set => SetValue(nameof(MessageId), x => x.MessageId = value, value);
        }

        public IDictionary<string, object> Headers
        {
            get => (IDictionary<string, object>)GetValue(nameof(Headers));
            set => SetValue(nameof(Headers), x => x.Headers = value, value);
        }

        public string Expiration
        {
            get => (string)GetValue(nameof(Expiration));
            set => SetValue(nameof(Expiration), x => x.Expiration = value, value);
        }

        public byte? DeliveryMode
        {
            get => (byte?)GetValue(nameof(DeliveryMode));
            set => SetValue(nameof(DeliveryMode), x => x.DeliveryMode = value.Value, value);
        }

        public string CorrelationId
        {
            get => (string)GetValue(nameof(CorrelationId));
            set => SetValue(nameof(CorrelationId), x => x.CorrelationId = value, value);
        }

        public string ContentType
        {
            get => (string)GetValue(nameof(ContentType));
            set => SetValue(nameof(ContentType), x => x.ContentType = value, value);
        }

        public string ContentEncoding
        {
            get => (string)GetValue(nameof(ContentEncoding));
            set => SetValue(nameof(ContentEncoding), x => x.ContentEncoding = value, value);
        }

        public string ClusterId
        {
            get => (string)GetValue(nameof(ClusterId));
            set => SetValue(nameof(ClusterId), x => x.ClusterId = value, value);
        }

        public string AppId
        {
            get => (string)GetValue(nameof(AppId));
            set => SetValue(nameof(AppId), x => x.AppId = value, value);
        }

        public string Type
        {
            get => (string)GetValue(nameof(Type));
            set => SetValue(nameof(Type), x => x.Type = value, value);
        }

        #endregion

        private object GetValue(string key)
        {
            if (setters.TryGetValue(key, out var value))
                return value.Value;

            return null;
        }

        private void SetValue<T>(string key, Action<IBasicProperties> setter, T value)
        {
            if (value == null)
            {
                if (setters != null)
                    setters.Remove(key);
            }
            else
            {
                if (setters == null)
                    setters = new Dictionary<string, (object, Action<IBasicProperties>)>();

                setters[key] = (value, setter);
            }
        }

        internal IBasicProperties Build(IModel channel)
        {
            if (setters == null)
                return null;
            
            var properties = channel.CreateBasicProperties();

            foreach (var (Value, Setter) in setters.Values)
                Setter(properties);

            return properties;
        }

        public MessageProperties Clone()
            =>  new MessageProperties()
            {
                setters = setters == null
                    ? null
                    : new Dictionary<string, (object Value, Action<IBasicProperties> Setter)>(setters)
            };
    }
}
