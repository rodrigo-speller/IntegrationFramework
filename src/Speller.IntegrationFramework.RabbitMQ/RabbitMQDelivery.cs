// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Speller.IntegrationFramework.RabbitMQ
{
    public class RabbitMQDelivery
    {
        private readonly IModel channel;
        internal readonly BasicDeliverEventArgs source;

        private readonly object sync = new object();
        private int rawDeliveryState;
        private ICollection<Exception> exceptions;

        internal RabbitMQDelivery(IModel channel, AcknowledgeMode acknowledgeMode, BasicDeliverEventArgs source)
        {
            this.channel = channel;
            this.source = source;

            rawDeliveryState = acknowledgeMode == AcknowledgeMode.Automatic
                ? (int)RabbitMQDeliveryState.AutoAcknowledged
                : (int)RabbitMQDeliveryState.Undefined;
        }

        public bool Redelivered
            => source.Redelivered;

        public string RoutingKey
            => source.RoutingKey;

        public ICollection<byte> Body
            => Array.AsReadOnly(source.Body);

        #region Properties

        public string UserId
            => source.BasicProperties.UserId;

        public string ReplyTo
            => source.BasicProperties.ReplyTo;

        public byte Priority
            => source.BasicProperties.Priority;

        public bool Persistent
            => source.BasicProperties.Persistent;

        public string MessageId
            => source.BasicProperties.MessageId;

        public IDictionary<string, object> Headers
            => source.BasicProperties.Headers;

        public string Expiration
            => source.BasicProperties.Expiration;

        public byte DeliveryMode
            => source.BasicProperties.DeliveryMode;

        public string CorrelationId
            => source.BasicProperties.CorrelationId;

        public string ContentType
            => source.BasicProperties.ContentType;

        public string ContentEncoding
            => source.BasicProperties.ContentEncoding;

        public string ClusterId
            => source.BasicProperties.ClusterId;

        public string AppId
            => source.BasicProperties.AppId;

        public string Type
            => source.BasicProperties.Type;

        public RabbitMQDeliveryState DeliveryState
            => (RabbitMQDeliveryState)rawDeliveryState;

        public IEnumerable<Exception> Exceptions
            => exceptions?.ToArray() ?? Array.Empty<Exception>();

        #endregion

        public async Task Acknowledge()
        {
            var previousState = await SafeAcknowledge();
            EnsureState(previousState, RabbitMQDeliveryState.Undefined);
        }

        public async Task<bool> TryAcknowledge()
        {
            var previousState = await SafeAcknowledge();
            return CompareState(previousState, RabbitMQDeliveryState.Undefined);
        }

        public async Task Unacknowledge()
        {
            var previousState = await SafeUnacknowledge();
            EnsureState(previousState, RabbitMQDeliveryState.Undefined);
        }

        public async Task<bool> TryUnacknowledge()
        {
            var previousState = await SafeUnacknowledge();
            return CompareState(previousState, RabbitMQDeliveryState.Undefined);
        }

        public async Task Reject()
        {
            var previousState = await SafeReject();
            EnsureState(previousState, RabbitMQDeliveryState.Undefined);
        }

        public async Task<bool> TryReject()
        {
            var previousState = await SafeReject();
            return CompareState(previousState, RabbitMQDeliveryState.Undefined);
        }
        
        public async Task SetException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            SafeSetException(exception);
        }

        private async Task<RabbitMQDeliveryState> SafeAcknowledge()
        {
            lock (sync)
            {
                try
                {
                    var previousState = TryExchangeDeliveryState(RabbitMQDeliveryState.Undefined, RabbitMQDeliveryState.Acknowledged);

                    if (previousState == RabbitMQDeliveryState.Undefined)
                        channel.BasicAck(source.DeliveryTag, false);

                    return previousState;
                }
                catch (Exception ex)
                {
                    SafeSetException(ex);
                    throw;
                }
            }
        }

        private async Task<RabbitMQDeliveryState> SafeUnacknowledge()
        {
            lock (sync)
            {
                try
                {
                    var previousState = TryExchangeDeliveryState(RabbitMQDeliveryState.Undefined, RabbitMQDeliveryState.Unacknowledged);

                    if (previousState == RabbitMQDeliveryState.Undefined)
                        channel.BasicNack(source.DeliveryTag, false, true);

                    return previousState;
                }
                catch (Exception ex)
                {
                    SafeSetException(ex);
                    throw;
                }
            }
        }

        private async Task<RabbitMQDeliveryState> SafeReject()
        {
            lock (sync)
            {
                try
                {
                    var previousState = TryExchangeDeliveryState(RabbitMQDeliveryState.Undefined, RabbitMQDeliveryState.Rejected);

                    if (previousState == RabbitMQDeliveryState.Undefined)
                        channel.BasicReject(source.DeliveryTag, false);

                    return previousState;
                }
                catch (Exception ex)
                {
                    SafeSetException(ex);
                    throw;
                }
            }
        }

        private void SafeSetException(Exception exception)
        {
            lock (sync)
            {
                if (exceptions == null)
                    exceptions = new LinkedList<Exception>();

                exceptions.Add(exception);
            }
        }

        private RabbitMQDeliveryState TryExchangeDeliveryState(RabbitMQDeliveryState expected, RabbitMQDeliveryState value)
            => (RabbitMQDeliveryState)Interlocked.CompareExchange(ref rawDeliveryState, (int)value, (int)expected);

        private bool CompareState(RabbitMQDeliveryState value, RabbitMQDeliveryState expected)
            => expected == value;

        private void EnsureState(RabbitMQDeliveryState value, RabbitMQDeliveryState expected)
        {
            if (CompareState(value, expected))
                throw new UnexpectedDeliveryStateException(value);
        }
    }   
}
