// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Converters
{
    internal class FieldTypeConverter : IConverter
    {
#nullable enable
        internal static FieldType Read(IMessagePackReader reader)
        {
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            var enumString = (string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

            switch (enumString)
            {
                case "str":
                    return FieldType.Str;
                case "*":
                    return FieldType.Star;
                case "scalar":
                    return FieldType.Scalar;
                case "string":
                    return FieldType.String;
                case "nil":
                    return FieldType.Nil;
                case "any":
                    return FieldType.Any;
                case "num":
                    return FieldType.Num;
                case "map":
                    return FieldType.Map;
                case "integer":
                    return FieldType.Integer;
                case "unsigned":
                    return FieldType.Unsigned;
                case "boolean":
                    return FieldType.Boolean;
                case "double":
                    return FieldType.Double;
                case "float":
                    return FieldType.Float;
                case "array":
                    return FieldType.Array;
                case "decimal":
                    return FieldType.Decimal;
                case "datetime":
                    return FieldType.Datetime;
                case "int":
                    return FieldType.Int;
                case "uint":
                    return FieldType.Uint;
                case "uuid":
                    return FieldType.Uuid;
                case "varbinary":
                    return FieldType.Varbinary;
                case "number":
                    return FieldType.Number;
                case "extension":
                    return FieldType.Extension;
                default:
                    throw ExceptionHelper.UnexpectedEnumUnderlyingType(typeof(FieldType), enumString);
            }
        }

        internal static void Write(FieldType value, [NotNull] IMessagePackWriter writer)
        {
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            switch (value)
            {
                case FieldType.Decimal:
                    stringConverter.Write("decimal", writer);
                    break;
                case FieldType.Number:
                    stringConverter.Write("number", writer);
                    break;
                case FieldType.Datetime:
                    stringConverter.Write("datetime", writer);
                    break;
                case FieldType.Uuid:
                    stringConverter.Write("uuid", writer);
                    break;
                case FieldType.Uint:
                    stringConverter.Write("uint", writer);
                    break;
                case FieldType.Int:
                    stringConverter.Write("int", writer);
                    break;
                case FieldType.Varbinary:
                    stringConverter.Write("varbinary", writer);
                    break;
                case FieldType.Star:
                    stringConverter.Write("*", writer);
                    break;
                case FieldType.Str:
                    stringConverter.Write("str", writer);
                    break;
                case FieldType.Num:
                    stringConverter.Write("num", writer);
                    break;
                case FieldType.String:
                    stringConverter.Write("string", writer);
                    break;
                case FieldType.Unsigned:
                    stringConverter.Write("unsigned", writer);
                    break;
                case FieldType.Any:
                    stringConverter.Write("any", writer);
                    break;
                case FieldType.Nil:
                    stringConverter.Write("nil", writer);
                    break;
                case FieldType.Integer:
                    stringConverter.Write("integer", writer);
                    break;
                case FieldType.Array:
                    stringConverter.Write("array", writer);
                    break;
                case FieldType.Map:
                    stringConverter.Write("map", writer);
                    break;
                case FieldType.Binary:
                    stringConverter.Write("binary", writer);
                    break;
                case FieldType.Boolean:
                    stringConverter.Write("boolean", writer);
                    break;
                case FieldType.Double:
                    stringConverter.Write("double", writer);
                    break;
                case FieldType.Float:
                    stringConverter.Write("float", writer);
                    break;
                case FieldType.Extension:
                    stringConverter.Write("extension", writer);
                    break;
                case FieldType.Scalar:
                    stringConverter.Write("scalar", writer);
                    break;
                default:
                    throw ExceptionHelper.EnumValueExpected(value.GetType(), value);
            }
        }

        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader);
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((FieldType)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
