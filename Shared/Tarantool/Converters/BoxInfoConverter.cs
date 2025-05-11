// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The box.info submodule provides access to information about a running <see cref="Tarantool"/> instance.
    /// </summary>
    public partial class BoxInfo
    {
        /// <summary>
        /// The <see cref="Tarantool"/> box info converter class.
        /// </summary>
#nullable enable
        internal class BoxInfoConverter : IConverter
        {
            private static BoxInfo? Read(IMessagePackReader reader)
            {
                var len = reader.ReadArrayLength();

                if (len != 1)
                {
                    throw ExceptionHelper.InvalidArrayLength(1, len);
                }

                var mapLength = reader.ReadMapLength();

                if (mapLength == uint.MaxValue)
                {
                    return null;
                }

                var stringConverter = ConverterContext.GetConverter(typeof(string));
                var longConverter = ConverterContext.GetConverter(typeof(long));
                var boolConverter = ConverterContext.GetConverter(typeof(bool));

                var result = new BoxInfo();
                for (var i = 0; i < mapLength; i++)
                {
                    var fieldName = stringConverter.Read(reader);
                    if (fieldName != null)
                    {
                        switch ((string)fieldName)
                        {
                            case "id":
                                result.Id = (long)(longConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                                break;
                            case "lsn":
                                result.Lsn = (long)(longConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                                break;
                            case "pid":
                                result.Pid = (long)(longConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                                break;
                            case "ro":
                                result.ReadOnly = (bool)(boolConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                                break;
                            case "uuid":
                                result.Uuid = new Guid((string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()));
                                break;
                            case "version":
                                result.Version = TarantoolVersion.Parse((string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()));
                                break;
                            default:
                                reader.SkipToken();
                                break;
                        }
                    }
                }

                return result;
            }

            public virtual void Write(object? value, [NotNull] IMessagePackWriter writer)
            {
                throw new NotImplementedException();
            }

            object? IConverter.Read([NotNull] IMessagePackReader reader)
            {
                return Read(reader);
            }
        }
    }
}
