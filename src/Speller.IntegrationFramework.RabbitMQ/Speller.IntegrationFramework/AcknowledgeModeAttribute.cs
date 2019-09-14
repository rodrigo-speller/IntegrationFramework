// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Speller.IntegrationFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AcknowledgeModeAttribute : Attribute
    {
        public const AcknowledgeMode DefaultMode = AcknowledgeMode.AfterHandling;

        internal readonly AcknowledgeMode mode;

        public AcknowledgeModeAttribute(AcknowledgeMode mode)
        {
            this.mode = mode;
        }

        internal static AcknowledgeMode FromType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var attribute = type.GetCustomAttribute<AcknowledgeModeAttribute>(true);

            if (attribute != null)
                return attribute.mode;

            return DefaultMode;
        }
    }
}
