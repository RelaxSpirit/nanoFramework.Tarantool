// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Stream;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// <see cref="Tarantool"/> SQL parameter class.
    /// </summary>
    public class SqlParameter
    {
        private readonly object value;
        private readonly string name;
        private readonly Type typeValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlParameter" /> class.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        public SqlParameter(object value)
        {
            this.name = string.Empty;
            this.value = value;
            this.typeValue = value.GetType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlParameter" /> class.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        /// <param name="name">Parameter name.</param>
        /// <exception cref="ArgumentException">If parameter name does not start with characters ':' or '@' or '$'.</exception>
        public SqlParameter(object value, string name)
            : this(value)
        {
            if (name[0] != ':' && name[0] != '@' && name[0] != '$')
            {
#if NANOFRAMEWORK_1_0
                throw new ArgumentException();
#else
                throw new ArgumentException("Name should start either with ':', '$' or '@'.", nameof(name));
#endif
            }

            this.name = name;
        }

        internal void Write([NotNull] IMessagePackWriter writer)
        {
            if (this.name != null)
            {
                writer.WriteMapHeader(1u);
                ConverterContext.GetConverter(typeof(string)).Write(this.name, writer);
            }

            ConverterContext.GetConverter(this.typeValue).Write(this.value, writer);
        }
    }
}
