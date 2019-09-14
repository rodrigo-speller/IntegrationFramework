// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using RabbitMQ.Client;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQBusServiceOptionsBuilderConnectionExtensions
    {
        public static RabbitMQBusServiceOptionsBuilder ConfigureConnection(this RabbitMQBusServiceOptionsBuilder builder, Action<ConnectionFactory> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            builder.ConfigureConnectionActions.Add(action);

            return builder;
        }

        public static RabbitMQBusServiceOptionsBuilder ConnectionString(this RabbitMQBusServiceOptionsBuilder builder, string connectionString)
        {
            var uri = new Uri(connectionString);

            builder.ConfigureConnectionActions.Add(x => x.Uri = uri);

            return builder;
        }
    }
}
