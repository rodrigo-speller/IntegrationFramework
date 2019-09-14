// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace Speller.IntegrationFramework.RabbitMQ
{
    [Serializable]
    class UnexpectedDeliveryStateException : InvalidOperationException
    {
        public UnexpectedDeliveryStateException(RabbitMQDeliveryState state)
            : this(state, "The current delivery state is invalid for operation execution.")
        { }

        public UnexpectedDeliveryStateException(RabbitMQDeliveryState deliveryState, string message)
            : base(message)
        {
            DeliveryState = deliveryState;
        }

        protected UnexpectedDeliveryStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            DeliveryState = (RabbitMQDeliveryState)info.GetValue(nameof(DeliveryState), typeof(RabbitMQDeliveryState));
        }

        public RabbitMQDeliveryState DeliveryState { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(DeliveryState), DeliveryState);

            base.GetObjectData(info, context);
        }
    }
}
