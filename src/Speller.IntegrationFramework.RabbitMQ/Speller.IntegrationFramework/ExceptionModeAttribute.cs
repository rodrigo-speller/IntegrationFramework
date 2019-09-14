// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Speller.IntegrationFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExceptionModeAttribute : Attribute
    {
        public const ExceptionMode DefaultMode = ExceptionMode.Reject;

        internal readonly ExceptionMode mode;

        public ExceptionModeAttribute(ExceptionMode mode)
        {
            this.mode = mode;
        }

        internal static ExceptionMode FromType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var attribute = type.GetCustomAttribute<ExceptionModeAttribute>(true);

            if (attribute != null)
                return attribute.mode;

            return DefaultMode;
        }
    }
}
