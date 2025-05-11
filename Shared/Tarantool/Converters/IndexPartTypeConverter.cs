using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Converters
{
    internal class IndexPartTypeConverter : IConverter
    {
        internal IndexPartType Read(IMessagePackReader reader)
        {
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            var enumString = (string)stringConverter.Read(reader);

            return enumString switch
            {
                "Str" => IndexPartType.Str,
                "Num" => IndexPartType.Num,
                _ => throw ExceptionHelper.UnexpectedEnumUnderlyingType(typeof(IndexPartType), enumString),
            };
        }

        internal void Write(IndexPartType value, [NotNull] IMessagePackWriter writer)
        {
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            switch (value)
            {
                case IndexPartType.Str:
                    stringConverter.Write("Str", writer);
                    break;
                case IndexPartType.Num:
                    stringConverter.Write("Num", writer);
                    break;
                default:
                    throw ExceptionHelper.EnumValueExpected(value.GetType(), value);
            }
        }

#nullable enable
        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader);
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            Write((IndexPartType)value!, writer);
        }
    }
}
