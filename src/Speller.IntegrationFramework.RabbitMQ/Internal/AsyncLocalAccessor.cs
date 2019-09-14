// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;

namespace Speller.IntegrationFramework.RabbitMQ.Internal
{
    internal class AsyncLocalAccessor<T>
    {
        private static readonly AsyncLocal<Holder> mainHolder = new AsyncLocal<Holder>();
        
        public T Value
        {
            get
            {
                var holder = mainHolder.Value;

                if (holder == null)
                    return default;

                return holder.Value;
            }
            set
            {
                var holder = mainHolder.Value;
                if (holder != null)
                    holder.Value = default;

                if (value != null)
                    mainHolder.Value = new Holder() { Value = value };
            }
        }

        private class Holder
        {
            public T Value;
        }
    }
}
