using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InfrastructureBase.Object
{
    public class JsonSerializerDefaultOption
    {
        public static JsonSerializerOptions Default
        {
            get
            {
                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;//忽略大小写
                                                           //基础类型处理通过客户端自定义重载
                options.Converters.Add(new TextJsonConverter.DateTimeParse());
                options.Converters.Add(new TextJsonConverter.IntParse());
                options.Converters.Add(new TextJsonConverter.DoubleParse());
                options.Converters.Add(new TextJsonConverter.DecimalParse());
                options.Converters.Add(new TextJsonConverter.FloatParse());
                options.Converters.Add(new TextJsonConverter.GuidParse());
                options.Converters.Add(new TextJsonConverter.BoolParse());
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; //响应驼峰命名
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;//中文乱码
                options.AllowTrailingCommas = true;//允许数组末尾多余的逗号
                return options;
            }
        }
    }
    public class TextJsonConverter
    {
        public class DateTimeParse : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return CommonTimeConvter(reader);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
        public class IntParse : JsonConverter<int>
        {
            public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    if (reader.TryGetInt32(out int value))
                        return value;
                    return default(int);
                }
                catch (Exception)
                {
                    return CommonNumberConvter(reader, int.Parse);
                }
            }

            public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }
        public class DecimalParse : JsonConverter<decimal>
        {
            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    if (reader.TryGetDecimal(out decimal value))
                        return value;
                    return default(decimal);
                }
                catch (Exception)
                {
                    return CommonNumberConvter(reader, decimal.Parse);
                }
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }
        public class DoubleParse : JsonConverter<double>
        {
            public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    if (reader.TryGetDouble(out double value))
                        return value;
                    else
                        return default(double);
                }
                catch (Exception)
                {
                    return CommonNumberConvter(reader, double.Parse);
                }
            }

            public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }
        public class FloatParse : JsonConverter<float>
        {
            public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    if (reader.TryGetDouble(out double value))
                        return (float)value;
                    else
                        return default(float);
                }
                catch (Exception)
                {
                    return CommonNumberConvter(reader, float.Parse);
                }
            }

            public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }
        public class GuidParse : JsonConverter<Guid>
        {
            public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    if (reader.TryGetGuid(out Guid value))
                        return value;
                    return Guid.Empty;
                }
                catch (Exception)
                {
                    return CommonNumberConvter(reader, Guid.Parse);
                }
            }

            public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value);
            }
        }
        public class BoolParse : JsonConverter<bool>
        {
            public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    return reader.GetBoolean();
                }
                catch (Exception)
                {
                    return CommonNumberConvter(reader, bool.Parse);
                }
            }

            public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            {
                writer.WriteBooleanValue(value);
            }
        }
        static T CommonNumberConvter<T>(Utf8JsonReader reader, Func<string, T> func)
        {
            try
            {
                var str = reader.GetString();
                if (!string.IsNullOrEmpty(str))
                    return func(str);
            }
            catch (Exception)
            {

            }
            return default;
        }

        static DateTime CommonTimeConvter(Utf8JsonReader reader)
        {

            try
            {
                var time = (long)reader.GetDouble();
                return DateTime.MinValue.Add(new TimeSpan(time));
            }
            catch (Exception)
            {

            }
            return default;
        }
    }
}
