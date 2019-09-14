// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Speller.IntegrationFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SubscribeAttribute : Attribute
    {
        public SubscribeAttribute(string queue = null)
        {
            Queue = queue;
        }

        public string Queue { get; }

        internal static SubscribeAttribute FromType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var name = InferQueueFromType(type);

            return new SubscribeAttribute(name);
        }

        private static string InferQueueFromType(Type type)
        {
            var name = type.Name;

            var ns = type.Namespace;
            if (!string.IsNullOrWhiteSpace(ns))
                name = $"{ns}.{name}";

            return name;
        }
    }
}
