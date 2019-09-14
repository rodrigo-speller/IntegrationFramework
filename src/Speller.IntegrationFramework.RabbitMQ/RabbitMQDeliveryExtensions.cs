// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Speller.IntegrationFramework.RabbitMQ
{
    public static class RabbitMQDeliveryExtensions
    {
        public static string AsString(this RabbitMQDelivery message)
        {
            var encodingName = message.source.BasicProperties.ContentEncoding;

            var encoding = encodingName != null
                ? Encoding.GetEncoding(encodingName)
                : Encoding.UTF8
                ;

            return AsString(message, encoding);
        }

        public static string AsString(this RabbitMQDelivery message, Encoding encoding)
        {
            var body = message.source.Body;

            if (body == null)
                return null;

            return encoding.GetString(body);
        }
    }
}
