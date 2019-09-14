// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Speller.IntegrationFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExchangeBindingAttribute : Attribute
    {
        public ExchangeBindingAttribute(string exchange)
            : this(exchange, string.Empty)
        { }

        public ExchangeBindingAttribute(string exchange, string routingKey)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
        }

        public string Exchange { get; }
        public string RoutingKey { get; }
    }
}
