using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SwishClient.JsonConverters
{
    public class JsonStringEnumConverterEx : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            //var converter = (JsonConverter)Activator.CreateInstance(
            //    typeof(JsonStringEnumConverterExInner<>).MakeGenericType(type),
            //    BindingFlags.Instance | BindingFlags.Public,
            //    binder: null,
            //    args: new object[] {options},
            //    culture: null);

            var converter = (JsonConverter)Activator.CreateInstance(typeof(JsonStringEnumConverterExInner<>).MakeGenericType(type));

            return converter;
        }

        private class JsonStringEnumConverterExInner<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
        {
            private readonly Dictionary<TEnum, string> enumToString = new Dictionary<TEnum, string>();
            private readonly Dictionary<string, TEnum> stringToEnum = new Dictionary<string, TEnum>();

            public JsonStringEnumConverterExInner()
            {
                var type = typeof(TEnum);

                var values = Enum.GetValues(type).Cast<TEnum>();

                foreach (var value in values)
                {
                    var enumMember = type.GetMember(value.ToString())[0];

                    var attr = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                        .Cast<EnumMemberAttribute>()
                        .FirstOrDefault();

                    if (attr?.Value != null)
                    {
                        enumToString.Add(value, attr.Value);
                        stringToEnum.Add(attr.Value, value);
                    }
                    else
                    {
                        enumToString.Add(value, value.ToString());
                        stringToEnum.Add(value.ToString(), value);
                    }
                }
            }

            public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var stringValue = reader.GetString();

                if (stringValue != null && stringToEnum.TryGetValue(stringValue, out var enumValue))
                {
                    return enumValue;
                }

                return default;
            }

            public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(enumToString[value]);
            }
        }
    }
}
