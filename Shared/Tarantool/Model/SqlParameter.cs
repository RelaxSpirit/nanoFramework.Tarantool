// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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
#nullable enable
        private readonly object _value;
        private readonly string _name;
        private readonly Type _typeValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlParameter" /> class.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        public SqlParameter(object value)
        {
            _name = string.Empty;
            _value = value;
            _typeValue = value.GetType();
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

            _name = name;
        }

        internal void Write([NotNull] IMessagePackWriter writer)
        {
            if (_name != null)
            {
                writer.WriteMapHeader(1u);
                ConverterContext.GetConverter(typeof(string)).Write(_name, writer);
            }

            ConverterContext.GetConverter(_typeValue).Write(_value, writer);
        }
    }
}
