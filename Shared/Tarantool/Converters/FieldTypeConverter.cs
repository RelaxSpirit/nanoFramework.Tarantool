// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
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
            var enumString = (string)(TarantoolContext.Instance.StringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

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
            switch (value)
            {
                case FieldType.Decimal:
                    TarantoolContext.Instance.StringConverter.Write("decimal", writer);
                    break;
                case FieldType.Number:
                    TarantoolContext.Instance.StringConverter.Write("number", writer);
                    break;
                case FieldType.Datetime:
                    TarantoolContext.Instance.StringConverter.Write("datetime", writer);
                    break;
                case FieldType.Uuid:
                    TarantoolContext.Instance.StringConverter.Write("uuid", writer);
                    break;
                case FieldType.Uint:
                    TarantoolContext.Instance.StringConverter.Write("uint", writer);
                    break;
                case FieldType.Int:
                    TarantoolContext.Instance.StringConverter.Write("int", writer);
                    break;
                case FieldType.Varbinary:
                    TarantoolContext.Instance.StringConverter.Write("varbinary", writer);
                    break;
                case FieldType.Star:
                    TarantoolContext.Instance.StringConverter.Write("*", writer);
                    break;
                case FieldType.Str:
                    TarantoolContext.Instance.StringConverter.Write("str", writer);
                    break;
                case FieldType.Num:
                    TarantoolContext.Instance.StringConverter.Write("num", writer);
                    break;
                case FieldType.String:
                    TarantoolContext.Instance.StringConverter.Write("string", writer);
                    break;
                case FieldType.Unsigned:
                    TarantoolContext.Instance.StringConverter.Write("unsigned", writer);
                    break;
                case FieldType.Any:
                    TarantoolContext.Instance.StringConverter.Write("any", writer);
                    break;
                case FieldType.Nil:
                    TarantoolContext.Instance.StringConverter.Write("nil", writer);
                    break;
                case FieldType.Integer:
                    TarantoolContext.Instance.StringConverter.Write("integer", writer);
                    break;
                case FieldType.Array:
                    TarantoolContext.Instance.StringConverter.Write("array", writer);
                    break;
                case FieldType.Map:
                    TarantoolContext.Instance.StringConverter.Write("map", writer);
                    break;
                case FieldType.Binary:
                    TarantoolContext.Instance.StringConverter.Write("binary", writer);
                    break;
                case FieldType.Boolean:
                    TarantoolContext.Instance.StringConverter.Write("boolean", writer);
                    break;
                case FieldType.Double:
                    TarantoolContext.Instance.StringConverter.Write("double", writer);
                    break;
                case FieldType.Float:
                    TarantoolContext.Instance.StringConverter.Write("float", writer);
                    break;
                case FieldType.Extension:
                    TarantoolContext.Instance.StringConverter.Write("extension", writer);
                    break;
                case FieldType.Scalar:
                    TarantoolContext.Instance.StringConverter.Write("scalar", writer);
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
