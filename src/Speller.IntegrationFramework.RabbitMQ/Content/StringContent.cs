// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace Speller.IntegrationFramework.RabbitMQ.Content
{
    public class StringContent : RawContent
    {
        private static Encoding defaultEncoding = Encoding.UTF8;
        public static Encoding DefaultEncoding
        {
            get => defaultEncoding;
            set => defaultEncoding = value
                ?? throw new ArgumentNullException(nameof(value));
        }

        public StringContent(string value)
            : this(value, DefaultEncoding)
        { }

        public StringContent(string value, Encoding encoding)
            : base(EnsureEncoding(ref encoding).GetBytes(value ?? string.Empty))
        {
            Properties.ContentEncoding = encoding.HeaderName;
        }

        private static Encoding EnsureEncoding(ref Encoding encoding)
        {
            if (encoding == null)
                encoding = DefaultEncoding;

            return encoding;
        }
    }
}
